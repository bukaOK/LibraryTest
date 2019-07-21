import CommonLayout from 'layouts/CommonLayout'
import BooksCatalog from 'pages/BooksCatalog'
import EditBook from 'pages/EditBook'
import EditGenre from 'pages/EditGenre'
import GenreList from 'pages/GenreList'
import EditClient from 'pages/EditClient'
import ClientsList from 'pages/ClientsList'

const routes = [
  {
    path: '/',
    component: CommonLayout,
    redirect: '/catalog',
    children: [
      { path: 'catalog', component: BooksCatalog },
      { path: 'books/register', component: EditBook },
      { path: 'books/edit/:id', component: EditBook },
      { path: 'genres/add', component: EditGenre },
      { path: 'genres/edit/:id', component: EditGenre },
      { path: 'genres/list', component: GenreList },
      { path: 'clients/register', component: EditClient },
      { path: 'clients/edit/:id', component: EditClient },
      { path: 'clients/list', component: ClientsList }
    ]
  }
]

// Always leave this as last one
if (process.env.MODE !== 'ssr') {
  routes.push({
    path: '*',
    component: () => import('pages/Error404.vue')
  })
}

export default routes
