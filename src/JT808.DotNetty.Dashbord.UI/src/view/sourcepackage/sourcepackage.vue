<template>
  <div>
        {{tmpResult}}
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
import { Add, Remove, GetAll } from "@/api/sourcePackage"
export default {
  name: 'sourcepackage',
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
