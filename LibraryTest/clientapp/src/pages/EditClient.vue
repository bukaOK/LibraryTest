<template>
  <div class="row">
    <div class="col-xs-12 col-md-6 q-mx-auto">
      <q-card>
        <q-card-section>
          <div class="text-h5">{{titleAction}} клиента</div>
        </q-card-section>
        <q-form ref="form" @submit="save">
          <q-card-section>
            <q-input label="ФИО" v-model="form.name" :rules="rules.name" />
            <q-input 
              label="Номер телефона" 
              mask="+7(###)###-##-##"
              unmasked-value
              v-model="form.phone" 
              :rules="rules.phone" 
            />
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
            <q-btn color="primary" label="Сохранить" type="submit" :loading="saveLoading" />
          </q-card-actions>
        </q-form>
      </q-card>
    </div>
  </div>
</template>
<script>
import editMixin from '../mixins/editMixin';
import formHelper from '../helpers/formHelper';

const validations = formHelper.validations
export default {
  name: 'EditClient',
  mixins: [editMixin],
  mounted(){
    if(this.isEdit){
      this.$api.get('/clients/get/' + this.id)
        .then(resp => {
          const respData = resp.data
          const form = this.form
          form.name = respData.name
          form.phone = respData.phone
        })
    }
  },
  data(){
    return {
      id: this.$route.params.id,
      form: {
        name: '',
        phone: ''
      },
      rules: {
        name: [
          v => validations.required(v, 'ФИО')
        ],
        phone: [
          v => validations.required(v, 'Номер телефона')
        ]
      },
      saveLoading: false,
      errors: []
    }
  },
  methods: {
    save(){
      this.$refs.form.validate().then(success => {
        if(success){
          const form = this.form
          const fd = new FormData()

          if(this.isEdit){
            fd.append('Id', this.id)
          }
          fd.append('Name', form.name)
          fd.append('Phone', form.phone)

          this.saveLoading = true
          const url = this.isEdit ? '/clients/update' : '/clients/add'
          this.$api.post(url, fd)
            .then(() => {
              this.$router.push('/clients/list')
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
    }
  }
}
</script>
