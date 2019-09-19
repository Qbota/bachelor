<template>
  <v-app id="inspire" >
    <v-content>
      <v-container
        class="fill-height"
        fluid
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col
            cols="12"
            sm="8"
            md="4"
          >
            <v-card class="elevation-12">
              <v-toolbar
               color="primary"
                flat
              >
                <v-toolbar-title>Login form</v-toolbar-title>
                <div class="flex-grow-1"></div>
              </v-toolbar>
              <v-card-text>
                <v-form>
                  <v-text-field
                    color="primary"
                    label="Login"
                    name="login"
                    type="text"
                    v-model="login"
                    @keyup.enter = "authenticate()"
                  ></v-text-field>
                  <v-text-field
                    color="primary"
                    id="password"
                    label="Password"
                    name="password"
                    type="password"
                    v-model="password"
                    @keyup.enter = "authenticate()"
                  ></v-text-field>
                </v-form>
              </v-card-text>
              <v-card-actions>
                <div class="flex-grow-1"></div>
                <v-btn @click="authenticate()" color="primary">Login</v-btn>
              </v-card-actions>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
  export default {
    name: 'LoginPage',
    data: () => ({
      drawer: null,
      login: "",
      password: ""
    }),
    
    methods: {
         authenticate: function() {
           if(this.login == "test" && this.password == "test"){
             this.$router.push({path: '/main'})
           }
         }
    }
  }
</script>