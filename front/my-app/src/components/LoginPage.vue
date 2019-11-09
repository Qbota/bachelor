<template>
  <v-app id="inspire" >
    <v-content > 
      <v-container
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col
            cols="5"
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
                    v-model="user.email"
                    @keyup.enter = "authenticate()"
                  ></v-text-field>
                  <v-text-field
                    color="primary"
                    id="password"
                    label="Password"
                    name="password"
                    type="password"
                    v-model="user.password"
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
import axios from 'axios';
  export default {
    name: 'LoginPage',
    data: () => ({
      drawer: null,
      user: {
        email: '',
        password: ''
      }
    }),
    
    methods: {
         authenticate() {
          axios.post('https://localhost:44340/api/user/user/Login', JSON.stringify(this.user),{
          headers: {
            'Content-Type': 'application/json'
          }
        }).then(response => {
          localStorage.setItem('token',response.data.token);
          localStorage.setItem('restaurantId', response.data.restaurantId)
          if(localStorage.getItem('restaurantId') == 0){
             this.$router.push('map')
           }
          else if(localStorage.getItem('restaurantId') != 0){
            this.$router.push('menu')
          }
        },error => {
          console.log(error);
        })
           
        }
    }
  }
</script>