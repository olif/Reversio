<template>
  <div class="signin-page">
    <div class="ui middle aligned center aligned grid">
      <div class="column">
        <h2 class="ui teal image header">
          <div class="content">
            Sign in to play
          </div>
        </h2>
        <form class="ui large form" v-on:submit.prevent="submit">
          <div class="ui segment">
            <div class="field">
              <div class="ui left icon input">
                <i class="user icon"></i>
                <input type="text" v-model="username" name="username" placeholder="Name">
              </div>
            </div>
            <button type="submit" class="ui fluid large teal submit button">Sign in</button>
          </div>
          <div class="ui error message"></div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
 export default {
   data: function () {
     return {
       username: ''
     }
   },
   methods: {
     submit (e) {
       this.$store.dispatch('SIGN_IN', this.username)
       .then(() => {
         this.$router.push('hello')
       })
       .catch(() => console.log('uh oh'))
     }
   }
 }
</script>

<style lang="scss">
  .signin-page {
    height: 100%;
    .grid {
      height: 100%;
    }
    .column {
      max-width: 450px;
    }
  }
</style>