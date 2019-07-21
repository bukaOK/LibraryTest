<template>
  <div class="row">
    <div class="col-xs-12 col-md-6 q-mx-auto">
      <q-card>
        <q-card-section>
          <div class="text-h5">{{titleAction}} жанр</div>
        </q-card-section>
        <q-form ref="form" @submit.prevent.stop="save">
          <q-card-section>
            <q-input label="Название" v-model="form.name" :rules="rules.name" />
            <q-banner v-for="error in errors" :key="error.id" class="bg-red text-white">
              <template v-slot:avatar>
                <q-icon name="error" />
              </template>
              {{error.message}}
            </q-banner>
          </q-card-section>
          <q-card-actions>
            <q-btn color="primary" type="submit" :loading="loading">Сохранить</q-btn>
          </q-card-actions>
        </q-form>
      </q-card>
    </div>
  </div>
</template>
<script>
import formHelper from '../helpers/formHelper'
import editMixin from '../mixins/editMixin'

const validations = formHelper.validations
export default {
  name: 'EditGenre',
  mixins: [editMixin],
  mounted(){
    if(this.id){
      this.$api.get('/genres/get/' + this.id)
      .then(resp => {
        this.form.name = resp.data.name
      })
    }
  },
  data(){
    return {
      id: this.$route.params.id,
      form: {
        name: ''
      },
      rules: {
        name: [
          v => validations.required(v, 'Название')
        ]
      },
      loading: false,
      errors: []
    }
  },
  methods: {
    save(){
      this.$refs.form.validate().then(success => {
        if(success){
          const fd = new FormData()
          fd.append('Name', this.form.name)
          if(this.isEdit){
            fd.append('Id', this.id)
          }

          this.loading = true
          const url = this.isEdit ? '/genres/update' : '/genres/add'
          this.$api.post(url, fd).then(() => {
            this.$router.push('/genres/list')
          }).catch(err => {
            if(err.response.status === 400){
              this.errors = formHelper.parseErrors(err.response.data)
            } else {
              const msg = err.response.data.message
              this.errors = msg ? [msg] : ['Внутренняя ошибка']
            }
          }).finally(() => {
            this.loading = false
          })
        }
      })
    }
  }
}
</script>
