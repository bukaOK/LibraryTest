<template>
  <div class="row justify-center q-col-gutter-md">
    <!-- Каталог -->
    <div class="col-xs-12 col-md-6">
      <q-card>
        <q-card-section class="row items-center">
          <div class="col-xs-6 col-md-8">
            <div class="text-h5">Каталог книг</div>
          </div>
          <div class="col-xs-6 col-md-4">
            <q-select dense options-dense filled label="Номер страницы" :options="pages" v-model="currentPage" />
          </div>
        </q-card-section>
        <q-card-section>
          <q-input
            v-model="searchName"
            label="Поиск по названию или автору"
          />
        </q-card-section>
        <q-card-section v-if="loading" class="text-center">
          <div class="text-h6">Загрузка...</div>
        </q-card-section>
        <q-list v-else-if="books.length">
          <q-item v-for="book in books" :key="book.id">
            <q-item-section>
              <q-item-label>{{book.name}}</q-item-label>
              <q-item-label caption>{{book.author}}</q-item-label>
            </q-item-section>
            <q-item-section side>
              <q-btn flat round icon="edit" color="grey-6" :to="`/books/edit/${book.id}`" />
            </q-item-section>
            <q-item-section side v-if="book.client">
              <q-btn flat round icon="person" color="grey-6" @click="showClient(book.client, book)" />
            </q-item-section>
            <q-item-section side v-else>
              <q-btn flat round icon="person_add" color="grey-6" @click="setBookClient(book.id)" />
            </q-item-section>
          </q-item>
        </q-list>
        <q-card-section v-else>
          <div class="text-h6">Книги не найдены</div>
        </q-card-section>
      </q-card>
    </div>
    <!-- Фильтр -->
    <div class="col-xs-12 col-md-3">
      <q-card>
        <q-card-section class="bg-blue text-white">
          <div class="text-h6">Фильтр</div>
        </q-card-section>
        <q-card-section>
          <div class="text-subtitle1">Статусы</div>
          <div class="column">
            <q-radio 
              v-for="status in availableStatuses" 
              :key="status.value" 
              :val="status.value" 
              :label="status.label"
              @input="loadBooks"
              v-model="bookStatus"
            />
          </div>
          <div class="text-subtitle1">Категории</div>
          <div class="column">
            <q-radio
              v-for="cat in availableCategories"
              :key="cat.value"
              :val="cat.value"
              :label="cat.label"
              @input="loadBooks"
              v-model="bookCategory"
            />
          </div>
          <div class="text-subtitle1">Жанры</div>
          <div class="column">
            <q-checkbox
              v-for="genre in genres"
              :key="genre.id"
              :val="genre.id"
              :label="genre.name"
              @input="loadBooks"
              v-model="selectedGenres"
            />
          </div>
          <q-input label="Год издания" v-model="year" @change="loadBooks" />
        </q-card-section>
      </q-card>
    </div>
    <!-- Диалоги -->
    <q-dialog v-model="currentClient.dialog">
      <q-card style="width: 350px">
        <q-card-section>
          <div class="text-h5">Клиент</div>
        </q-card-section>
        <q-list>
          <q-item>
            <q-item-section>
              <q-item-label class="text-subtitle1">
                {{currentClient.name}}
              </q-item-label>
              <q-item-label caption class="text-subtitle2">
                {{currentClient.phone}}
              </q-item-label>
            </q-item-section>
          </q-item>
        </q-list>
        <q-card-actions>
          <q-btn label="Вернуть книгу" flat color="red" :loading="currentClient.saveLoading" @click="removeBookClient" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="setClientForm.showDialog">
      <q-card style="width: 350px">
        <q-card-section>
          <div class="text-h5">Задать клиента для книги</div>
        </q-card-section>
        <q-card-section class="column q-gutter-sm">
          <q-select
            v-model="setClientForm.selectedClient"
            label="Имя клиента"
            use-input
            fill-input
            option-label="name"
            option-value="id"
            :options="setClientForm.clients"
            @filter="searchClientChange"
          >
            <template v-slot:no-option="{inp}">
              <q-item v-if="inp && inp.length">
                <q-item-section class="text-grey">Ничего не найдено</q-item-section>
              </q-item>
            </template>
            <template v-slot:option="{opt, itemEvents}">
              <q-item v-on="itemEvents" clickable>
                <q-item-section>
                  <q-item-label>{{opt.name}}</q-item-label>
                  <q-item-label caption>{{opt.phone}}</q-item-label>
                </q-item-section>
              </q-item>
            </template>
          </q-select>
          <div class="text-h6">Выбор даты возвращения</div>
          <q-date v-model="setClientForm.date" />
        </q-card-section>
        <q-card-actions>
          <q-btn label="Отдать клиенту" color="primary" flat :loading="setClientForm.saveLoading" @click="setBookClientSave" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>
<script>
export default {
  name: 'BooksCatalog',
  mounted(){
    this.loadBooks()
    this.loadGenres()
  },
  data(){
    return {
      loading: true,
      currentPage: 1,
      searchName: '',
      bookCategory: 0,
      bookStatus: 0,
      year: null,
      pagesCount: 1,
      books: [],
      genres: [],
      selectedGenres: [],
      currentClient: {
        dialog: false,
        saveLoading: false,
        name: '',
        phone: '',
        bookId: null
      },
      setClientForm: {
        showDialog: false,
        date: null,
        selectedClient: null,
        searchLoading: false,
        saveLoading: false,
        bookId: null,
        clients: []
      }
    }
  },
  computed: {
    availableStatuses(){
      return ['Все', 'В библиотеке', 'У клиента'].map((x, ind) => {
        return {
          label: x,
          value: ind
        }
      })
    },
    availableCategories(){
      return ['Все', 'Сборник рассказов', 'Классика(сборник томов)'].map((x, ind) => {
        return {
          label: x,
          value: ind
        }
      })
    },
    pages(){
      const arr = []
      for(let i = 0; i < this.pagesCount; i++){
        arr.push({
          label: i + 1,
          value: i + 1
        })
      }
      return arr
    }
  },
  watch: {
    searchName(val){
      setTimeout(() => {
        if(val === this.searchName){
          this.loadBooks()
        }
      }, 400);
    }
  },
  methods: {
    loadBooks(){
      this.loading = true

      const fd = new FormData()
      fd.append('Page', this.currentPage)
      fd.append('Name', this.searchName)
      if(this.bookCategory){
        fd.append('Category', this.bookCategory)
      }
      if(this.bookStatus){
        fd.append('Status', this.bookStatus)
      }
      if(this.year){
        fd.append('Year', this.year)
      }
      if(this.selectedGenres.length){
        for(let genre of this.selectedGenres){
          fd.append('Genres[]', genre)
        }
      }
      this.$api.post('/books/getlist', fd)
        .then(resp => {
          this.pagesCount = resp.data.pagesCount
          this.books = resp.data.books
        }).catch(() => {
          this.$q.notify({
            message: 'Ошибка при загрузке',
            color: 'red',
            textColor: 'white'
          })
        }).finally(() => {
          this.loading = false
        })
    },
    removeBookClient(){
      const bookId = this.currentClient.bookId

      this.currentClient.saveLoading = true
      this.$api.delete('/books/removebookclient/' + bookId).then(() => {
        this.$q.notify({
          message: 'Книга успешно отдана',
          color: 'green',
          textColor: 'white'
        })
        const book = this.books.find(x => x.id === bookId)
        book.client = null
      }).catch(() => {
        this.$q.notify({
          message: 'Ошибка при запросе',
          color: 'red',
          textColor: 'white'
        })
      }).finally(() => {
        this.currentClient.dialog = false
        this.currentClient.saveLoading = false
      })
    },
    setBookClient(bookId){
      const date = new Date()
      const form = this.setClientForm

      form.bookId = bookId
      form.showDialog = true

      const mday = date.getDate() + 1
      const month = date.getMonth() + 1
      form.date = `${date.getFullYear()}/${month < 10 ? '0' + month : month}/${mday < 10 ? '0' + mday : mday}`
    },
    setBookClientSave(){
      const fd = new FormData()
      const form = this.setClientForm

      fd.append('BookId', form.bookId)
      fd.append('ClientId', form.selectedClient.id)
      fd.append('EndDate', form.date)

      form.saveLoading = true
      this.$api.post('/books/setclient', fd).then(resp => {
        this.$q.notify({
          message: 'Клиент успешно задан',
          color: 'green',
          textColor: 'white'
        })
        const book = this.books.find(x => x.id === form.bookId)
        book.client = resp.data.client
      }).finally(() => {
        this.setClientForm.saveLoading = false
        this.setClientForm.showDialog = false
      })
    },
    searchClientChange(val, update){
      this.setClientForm.searchLoading = true

      this.$api.get('/clients/search?name=' + val).then(resp => {
        update(() => {
          console.log(resp.data)
          this.setClientForm.clients = resp.data.clients
          console.log(this.setClientForm.clients)
        })
      }).catch(() => {
        this.$q.notify({
          message: 'Ошибка при поиске',
          color: 'red'
        })
      }).finally(() => {
        this.setClientForm.searchLoading = false
      })
    },
    showClient(client, book){
      this.currentClient.name = client.name
      this.currentClient.phone = client.phone
      this.currentClient.bookId = book.id
      this.currentClient.dialog = true
    },
    loadGenres(){
      this.$api.get('/genres/getall').then(resp => {
        this.genres = resp.data
      })
    }
  }
}
</script>
