<template>
  <div>
    <Card>
      {{tmpResult}}
      <tables
        ref='tables'
        searchable
        search-place='top'
        v-model='tableData'
        :columns='columns'
        @on-delete='handleDelete'
      />
    </Card>
  </div>
</template>

<script>
import { mapMutations } from 'vuex'
import Tables from '_c/tables'
import { Add, Remove, GetAll } from '@/api/sourcePackage'
export default {
  name: 'sourcepackage',
  components: {
    Tables
  },
  data () {
    return {
      tmpResult: null,
      columns: [
        { title: '远程IP地址', key: 'RemoteAddress' },
        { title: '通道是否注册', key: 'Registered', render: function (h,params){  
                  return h('span', params.row.Registered? '是' : '否')
                }
        },
        { title: '通道是否活动', key: 'Active', render: function (h,params){  
                  return h('span', params.row.Active? '是' : '否')
                }
        },
        { title: '通道是否打开', key: 'Open', render: function (h,params){  
                  return h('span', params.row.Open? '是' : '否')
                }
        },
        {
          title: '操作',
          key: 'handle',
          options: ['delete'],
          button: [
            (h, params, vm) => {
              return h(
                'Poptip',
                {
                  props: {
                    confirm: true,
                    title: '你确定要删除吗?'
                  },
                  on: {
                    'on-ok': () => {
                      vm.$emit('on-delete', params)
                      vm.$emit(
                        'input',
                        params.tableData.filter(
                          (item, index) => index !== params.row.initRowIndex
                        )
                      )
                    }
                  }
                },
                [h('Button', '自定义删除')]
              )
            }
          ]
        }
      ],
      tableData: []
    }
  },
  methods: {
    ...mapMutations(['closeTag']),
    handleDelete (params) {
      console.log(params)
    }
  },
  mounted () {
    GetAll()
      .then(res => {
        this.tmpResult = res
        this.tableData = res.data.Data
      })
      .catch(err => {
        console.log(err)
      })
  }
}
</script>

<style>
</style>
