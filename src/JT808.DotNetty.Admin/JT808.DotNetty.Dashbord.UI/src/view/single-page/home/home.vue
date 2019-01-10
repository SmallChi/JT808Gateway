<template>
  <div>
    <Row :gutter="20">
      <i-col :xs="12" :md="6" :lg="6" v-for="(infor, i) in inforCardData" :key="`infor-${i}`" style="height: 120px;padding-bottom: 10px;">
        <infor-card shadow :color="infor.color" :icon="infor.icon" :icon-size="36">
          <count-to :end="infor.count" count-class="count-style"/>
          <p>{{ infor.title }}</p>
        </infor-card>
      </i-col>
    </Row>
    <Row :gutter="20" style="margin-top: 10px;">
      <i-col :md="24" :lg="24" style="margin-bottom: 20px;">
        <Card shadow>
          <chart-bar style="height: 300px;" :value="barData" text="每周车辆在线情况"/>
        </Card>
      </i-col>
    </Row>
  </div>
</template>

<script>
import InforCard from '_c/info-card'
import CountTo from '_c/count-to'
import { ChartBar } from '_c/charts'
export default {
  name: 'home',
  components: {
    InforCard,
    CountTo,
    ChartBar
  },
  data () {
    return {
      inforCardData: [
        { title: '车辆总数', icon: 'md-locate', count: 800, color: '#19be6b' },
        { title: '车辆在线数', icon: 'md-person-add', count: 793, color: '#2d8cf0' },
        { title: '接收包成功数', icon: 'md-help-circle', count: 14200, color: '#ff9900' },
        { title: '接收包失败数', icon: 'md-share', count: 657, color: '#ed3f14' }
      ],
      barData: {
        Mon: 13253,
        Tue: 34235,
        Wed: 26321,
        Thu: 12340,
        Fri: 24643,
        Sat: 1322,
        Sun: 1324
      }
    }
  },
  mounted () {
    //
  }
}
</script>

<style lang="less">
.count-style{
  font-size: 50px;
}
</style>
