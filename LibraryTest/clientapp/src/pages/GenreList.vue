<template>
  <div class="row">
    <div class="col-xs-12 col-md-6 q-mx-auto">
      <q-card>
        <q-card-section class="bg-blue text-white">
          <div class="text-h6">Жанры</div>
        </q-card-section>
        <q-card-section>
          <q-list v-if="genres.length">
            <q-item v-for="genre in genres" :key="genre.id">
              <q-item-section>
                <q-item-label class="text-subtitle1">{{genre.name}}</q-item-label>
              </q-item-section>
              <q-item-section side>
                <q-btn round flat icon="edit" :to="`/genres/edit/${genre.id}`" />
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>
      </q-card>
    </div>
  </div>
</template>
<script>
export default {
  name: 'GenreList',
  mounted(){
    this.$api.get('/genres/getall').then(resp => {
      this.genres = resp.data
    })
  },
  data(){
    return {
      genres: []
    }
  }
}
</script>
