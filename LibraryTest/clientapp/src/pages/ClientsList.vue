<template>
  <div class="row">
    <div class="col-xs-12 col-md-6 q-mx-auto">
      <q-card>
        <q-card-section class="row items-center">
          <div class="col-xs-6 col-md-8">
            <div class="text-h5">Список клиентов</div>
          </div>
          <div class="col-xs-6 col-md-4">
            <q-select dense options-dense filled label="Номер страницы" :options="pages" v-model="currentPage" />
          </div>
        </q-card-section>
        <q-card-section>
          <q-form @submit="findClientByPhone" class="row items-center q-col-gutter-md">
            <div class="col-xs-12 col-md-8">
              <q-input 
                mask="+7(###)###-##-##" 
                unmasked-value 
                dense 
                label="Найти по номеру телефона"
                v-model="searchPhone" 
              />
            </div>
            <div class="col-xs-12 col-md-4">
              <q-btn 
                label="Найти" 
                color="primary" 
                class="full-width" 
                :loading="loading"
                type="submit"
              />
            </div>
          </q-form>
        </q-card-section>
        <q-card-section>
          <q-input dense label="Найти по имени" v-model="searchName" />
        </q-card-section>
        <q-card-section>
          <div class="text-subtitle1">Виды клиентов</div>
          <div class="q-gutter-md">
            <q-radio 
              v-for="item in availableClientTypes" 
              :key="item.value" 
              :label="item.label" 
              :val="item.value"
              v-model="clientType"
              @input="loadClients"
            />
          </div>
        </q-card-section>
        <q-card-section v-if="loading" class="text-center">
          <div class="text-h5">Загрузка...</div>
        </q-card-section>
        <q-list v-else-if="clients.length">
          <q-item v-for="client in clients" :key="client.id">
            <q-item-section>
              <q-item-label class="text-h6">{{client.name}}</q-item-label>
              <q-item-label caption class="text-subtitle1">{{client.phone}}</q-item-label>
            </q-item-section>
            <q-item-section side>
              <q-btn round flat icon="edit" class="text-grey-5" :to="`/clients/edit/${client.id}`" />
            </q-item-section>
            <q-item-section side>
              <q-btn round flat icon="book" class="text-grey-5" @click="showClientBooksDialog(client.id)" />
            </q-item-section>
          </q-item>
        </q-list>
        <q-card-section v-else>
          <div class="text-h5">Клиенты не найдены</div>
        </q-card-section>
      </q-card>
    </div>
    <q-dialog v-model="clientBooks.dialog">
      <q-card style="width: 380px">
        <q-card-section>
          <div class="text-h5">Книги клиента</div>
        </q-card-section>
        <q-card-section class="text-center" v-if="clientBooks.loading">
          <div class="text-h6">Загрузка...</div>
        </q-card-section>
        <q-list v-else-if="clientBooks.books.length">
          <q-item v-for="book in clientBooks.books" :key="book.id">
            <q-item-section>
              <q-item-label class="text-subtitle1">{{book.name}}</q-item-label>
              <q-item-label caption>{{book.author}}</q-item-label>
            </q-item-section>
          </q-item>
        </q-list>
        <q-card-section v-else>
          <div class="text-h6">Книг нет</div>
        </q-card-section>
      </q-card>
    </q-dialog>
  </div>
</template>
<script>
import clientTypes from '../enums/clientTypes'

export default {
  name: 'ClientsList',
  mounted(){
    this.loadClients()
  },
  data(){
    return {
      pagesCount: 0,
      currentPage: 1,
      searchName: '',
      searchPhone: '',
      clients: [],
      clientType: clientTypes.all,
      loading: false,
      clientBooks: {
        clientId: null,
        dialog: false,
        books: [],
        loading: false
      }
    }
  },
  computed: {
    pages(){
      const arr = []
      for(let i = 0; i < this.pagesCount; i++){
        arr.push({
          label: i + 1,
          value: i + 1
        })
      }
      return arr
    },
    availableClientTypes(){
      return ['Все', 'Взяли книги', 'Просрочили'].map((src, ind) => {
        return {
          label: src,
          value: ind
        }
      })
    }
  },
  watch: {
    searchName(val){
      setTimeout(() => {
        if(this.searchName === val){
          this.loadClients()
        }
      }, 400)
    }
  },
  methods: {
    loadClients(){
      this.loading = true
      this.$api.get(`/clients/getlist?page=${this.currentPage}&name=${this.searchName}&clientType=${this.clientType}`)
        .then(resp => {
          this.pagesCount = resp.data.pagesCount
          this.clients = resp.data.clients
        }).catch(() => {
          this.$q.notify({
            message: 'Ошибка при загрузке клиентов',
            color: 'red',
            textColor: 'white'
          })
        }).finally(() => {
          this.loading = false
        })
    },
    findClientByPhone(){
      this.loading = true
      this.$api.get('/clients/getbyphone?phone=' + this.searchPhone)
        .then(resp => {
          if(resp.data){
            this.clients = [resp.data]
          } else {
            this.clients = []
          }
          this.pagesCount = 1
        }).catch(() => {
          this.$q.notify({
            message: 'Ошибка при поиске клиента по телефону',
            color: 'red',
            textColor: 'white'
          })
        }).finally(() => {
          this.loading = false
        })
    },
    showClientBooksDialog(clientId){
      const cb = this.clientBooks

      cb.loading = true
      cb.dialog = true
      this.$api.get('/books/getbyclient/' + clientId)
        .then(resp => {
          cb.books = resp.data
        }).finally(() => {
          cb.loading = false
        })
    }
  }
}
</script>
