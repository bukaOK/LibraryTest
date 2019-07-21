<template>
  <q-layout view="hHh lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          @click="leftDrawerOpen = !leftDrawerOpen"
          aria-label="Menu"
        >
          <q-icon name="menu" />
        </q-btn>

        <q-toolbar-title>
          Библиотека
        </q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      bordered
    >
      <q-list>
        <q-item v-for="item in drawerLinks" :to="item.url" :key="item.url">
          <q-item-section>
            <q-item-label>{{item.title}}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <q-page-container class="bg-grey-2">
      <q-page padding>
        <router-view />
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script>
import { openURL } from 'quasar'

export default {
  name: 'CommonLayout',
  data () {
    return {
      leftDrawerOpen: this.$q.platform.is.desktop,
      drawerLinks: [
        {
          title: 'Каталог',
          url: '/catalog'
        },
        {
          title: 'Клиенты',
          url: '/clients/list'
        },
        {
          title: 'Жанры',
          url: '/genres/list'
        },  
        {
          title: 'Добавить книгу',
          url: '/books/register'
        },
        {
          title: 'Добавить жанр',
          url: '/genres/add'
        },
        {
          title: 'Добавить клиента',
          url: '/clients/register'
        }
      ]
    }
  },
  methods: {
    openURL
  }
}
</script>

<style>
</style>
