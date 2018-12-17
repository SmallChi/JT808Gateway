<template>
  <div>
        {{tmpResult}}
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
import { GetAll } from "@/api/session"
export default {
  name: 'session',
  data () {
    return {
      tmpResult: null
    }
  },
  methods: {
    ...mapMutations([
      'closeTag'
    ])
  },
  mounted () {
    GetAll().then(res => {
      this.tmpResult = res
    }).catch(err => {
      console.log(err)
    })
  }
}
</script>

<style>

</style>
