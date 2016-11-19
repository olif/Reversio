<template>
  <span class="disc" v-bind:class="discType"></span>
</template>

<script>
export default {
  props: ['discTypeNr'],
  computed: {
    discType: function () {
      switch (this.discTypeNr) {
        case -1:
          return 'black'
        case 0:
          return 'empty'
        case 1:
          return 'white'
      }
      console.log(this.discTypeNr)
    }
  }
}
</script>

<style lang="scss" scoped>
  .disc {
    display: inline-block;
    width: 60px;
    height: 60px;
    margin: 6px;
    border-radius: 60px
  }
  
  .disc.empty {
    background: none;
  }

  .disc.black {
    background-color: #000;
    box-shadow: inset 0px 0px 0px 2px rgba(255,255,255,0.2)
  }

  .disc.white {
    background-color: #fff;
    box-shadow: inset 0px 0px 0px 2px rgba(0,0,0,0.2)
  }
</style>