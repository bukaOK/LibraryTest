<template>
  <div class="row">
    <div class="col-xs-12 col-md-6 q-mx-auto">
      <q-card>
        <q-card-section>
          <div class="text-h5">{{titleAction}} книгу</div>
        </q-card-section>
        <q-form ref="form" @submit="save">
          <q-card-section>
            <q-input label="Название" v-model="form.name" :rules="rules.name" />
            <q-input label="Автор" v-model="form.author" :rules="rules.name" />
            <q-input label="Год издания" v-model="form.year" :rules="rules.year" />
            <q-input label="ISBN" v-model="form.isbn" :rules="rules.isbn" />
            <q-input label="Количество страниц" v-model="form.pagesCount" :rules="rules.pagesCount" />
            <q-select 
              outlined 
              label="Жанр" 
              v-model="form.genre" 
              option-value="id" 
              option-label="name" 
              :options="availableGenres"
              :rules="rules.genre" 
            />
            <div style="margin-top: 1.5em">
              <q-radio v-for="item in categories" v-model="form.category" :label="item.label" :val="item.value" :key="item.value" />
            </div>
            <div v-if="isClassicBook">
              <q-input label="Количество глав" v-model="form.volCount" :rules="rules.volCount" />
            </div>
            <div v-else class="column q-gutter-md">
              <div class="text-h6">Рассказы</div>
              <q-card v-for="(story, ind) in stories" :key="ind">
                <q-card-section>
                  <q-input label="Название рассказа" v-model="story.name" />
                  <q-input type="textarea" label="Описание рассказа" v-model="story.description" />
                </q-card-section>
                <q-card-actions>
                  <q-btn color="red" label="Удалить" @click="removeStory(ind)" />
                </q-card-actions>
              </q-card>
              <q-btn label="Добавить рассказ" color="primary" @click="addStory" />
            </div>
          </q-card-section>
          <q-card-section v-if="errors.length">
            <q-banner v-for="error in errors" :key="error.id" class="bg-red text-white">
              <template v-slot:avatar>
                <q-icon name="error" />
              </template>
              {{error.message}}
            </q-banner>
          </q-card-section>
          <q-card-actions>
            <q-btn label="Сохранить" :loading="saveLoading" type="submit" color="blue" />
          </q-card-actions>
        </q-form>
      </q-card>
    </div>
  </div>
</template>
<script>
import formHelper from '../helpers/formHelper'
import editMixin from '../mixins/editMixin'
import bookCategories from '../enums/bookCategories'

const validations = formHelper.validations
export default {
  name: 'EditBook',
  mixins: [editMixin],
  mounted(){
    this.$api.get('/genres/getall').then(resp => {
      this.availableGenres = resp.data

      if(this.id){
        this.loadBook()
      }
    })
  },
  data(){
    return {
      id: this.$route.params.id,
      form: {
        name: '',
        isbn: '',
        year: null,
        pagesCount: null,
        category: 1,
        author: '',
        genre: null,
        // Only if category=2
        volCount: null,
        // Only if category=1
      },
      stories: [],
      availableGenres: [],
      categories: [
        { label: 'Сборник рассказов', value: bookCategories.stories },
        { label: 'Классическая', value: bookCategories.classic }
      ],
      rules: {
        name: [
          v => validations.required(v, 'Название')
        ],
        author: [
          v => validations.required(v, 'Автор')
        ],
        year: [
          v => validations.required(v, 'Год'),
          v => validations.number(v)
        ],
        isbn: [
          v => validations.required(v, 'ISBN'),
          v => validations.number(v)
        ],
        pagesCount: [
          v => validations.required(v, 'Количество страниц')
        ],
        volCount: [
          v => this.isClassicBook && validations.required(v, 'Количество глав')
        ],
        genre: [
          v => validations.required(v, 'Жанр')
        ]
      },
      saveLoading: false,
      bookLoading: false,
      errors: []
    }
  },
  computed: {
    isClassicBook(){
      return this.form.category === bookCategories.classic
    }
  },
  methods: {
    save(){
      this.$refs.form.validate().then(success => {
        if(success) {
          const fd = new FormData()
          const form = this.form
          if(this.isEdit){
            fd.append('Id', this.id)
          }
          fd.append('Name', form.name)
          fd.append('Year', form.year)
          fd.append('ISBN', form.isbn)
          fd.append('PagesCount', form.pagesCount)
          fd.append('GenreId', form.genre.id)
          fd.append('Author', form.author)
          if(this.isClassicBook){
            fd.append('VolCount', form.volCount)
          } else {
            if(!this.stories.length){
              this.errors = ['Заполните хотя бы один рассказ']
              return;
            }
            for(let i = 0; i < this.stories.length; i++){
              const story = this.stories[i]
              if(story.id){
                fd.append(`Stories[${i}].Id`, story.id)
              }
              fd.append(`Stories[${i}].Name`, story.name)
              fd.append(`Stories[${i}].Description`, story.description)
            }
          }
          fd.append('Category', form.category)

          this.saveLoading = true
          const url = this.isEdit ? '/books/update' : '/books/add'
          this.$api.post(url, fd)
            .then(() => {
              this.$router.push('/catalog')
            }).catch(err => {
              if(err.response.status === 400){
                this.errors = formHelper.parseErrors(err.response.data)
              } else {
                const msg = err.response.data.message
                this.errors = msg ? [msg] : 'Внутренняя ошибка'
              }
            }).finally(() => {
              this.saveLoading = false
            })
        }
      })
    },
    loadBook(){
      return this.$api.get('/books/getforedit/' + this.id)
        .then(resp => {
          const respData = resp.data
          const form = this.form

          form.name = respData.name
          form.year = respData.year
          form.isbn = respData.isbn
          form.pagesCount = respData.pagesCount
          form.genre = this.availableGenres.find(x => x.id === respData.genreId) 
          form.author = respData.author
          form.category = respData.category
          if(form.category == bookCategories.classic){
            form.volCount = respData.volCount
          } else {
            this.stories = respData.stories
          }
        })
    },
    addStory(){
      this.stories.push({
        name: '',
        description: '',
        id: null
      })
    },
    removeStory(index){
      this.stories.splice(index, 1)
    }
  }
}
</script>

