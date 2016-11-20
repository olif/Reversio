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
             <div class="field">
                <div class="ui left icon input">
                  <i class="lock icon"></i>
                  <input type="password" v-model="password" name="password" placeholder="Password" />
                </div>
            </div>
            <button type="submit" class="ui fluid large teal submit button" disabled>Sign in</button>
          </div>
          <div class="ui error message"></div>
          <div class="ui segment">
            <button type="button" @click="signinAsGuest()" class="ui fluid large pink submit button">Sign in as guest</button>
           </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
 export default {
   data: function () {
     return {
       username: '',
       password: ''
     }
   },
   methods: {
     submit (e) {
       this.$store.dispatch('SIGN_IN', this.username)
       .then(() => {
         this.$router.push('hello')
       })
       .catch(() => console.log('uh oh'))
     },

     signinAsGuest (e) {
       this.$store.dispatch('SIGN_IN_AS_GUEST')
       .then(() => {
         this.$router.push('gamearea')
       })
       console.log('signing in as guest')
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