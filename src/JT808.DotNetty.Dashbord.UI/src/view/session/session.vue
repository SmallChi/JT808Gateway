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
import { GetAll } from '@/api/session'
export default {
  name: 'session',
  components: {
    Tables
  },
  data () {
    return {
      tmpResult: null,
      columns: [
        { title: '通道Id', key: 'ChannelId' },
        { title: '终端设备号', key: 'TerminalPhoneNo' },
        { title: '上线时间', key: 'StartTime' },
        { title: '最后登录时间', key: 'LastActiveTime' },
        { title: 'WebApi端口号', key: 'WebApiPort' },
        { title: '设备远程地址', key: 'RemoteAddressIP' },
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
