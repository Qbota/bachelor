<template>
  <v-app>
    <v-card class="ma-12">
      <v-container>
        <v-form v-model="valid" ref="form" :lazy-validation="true" class="ma-5">
          <v-row justify="center">
            <v-col cols="10">
              <v-text-field v-model="login" label="Email" :rules="loginRules" required></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="10">
              <v-text-field v-model="password" label="Password" :rules="passwordRules" type="password" required ></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="10">
              <v-text-field v-model="second_password" label="Repeat password" :rules="secondPasswordRules" type="password" required></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="10">
              <v-text-field v-text="'Are you owner of a restaurant?'"/>
              <v-checkbox v-model="owner"/>
            </v-col>
          </v-row>
          <v-container v-if="owner">
          <v-row justify="center">
            <v-col cols="10">
              <v-text-field v-model="name" label="Name" :rules="nameRules"  required></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="5">
              <v-text-field v-model="address_street" label="Street" :rules="streetRules"  required></v-text-field>
            </v-col>
            <v-col cols="5">
              <v-text-field v-model="address_number" label="Number" :rules="numberRules" required></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="5">
              <v-text-field v-model="address_postal_code" label="Postal Code" :rules="codeRules"   required></v-text-field>
            </v-col>
            <v-col cols="5">
              <v-text-field v-model="address_city" label="City" :rules="cityRules" required></v-text-field>
            </v-col>
          </v-row>
          <v-row justify="center">
            <v-col cols="5"></v-col>
            <v-col cols="5">
              <v-text-field v-model="address_country" label="Country" :rules="codeRules" required></v-text-field>
            </v-col>
          </v-row>
          </v-container>
          <v-row justify="center">
            <v-btn class="ma-1" @click="clearInput()">Clear</v-btn>
            <v-btn :disabled="!valid" class="ma-1" @click="register()" :loading="loading">Register</v-btn>
          </v-row>
        </v-form>
      </v-container>
    </v-card>
  </v-app>
</template>

<script>
import axios from 'axios';
export default {
  name: "RegisterForm",
  data() {
    return {
      valid: true,
      loading: false,
      owner: false,
      name: "",
      restaurantId: 0,
      lng: 0.0,
      lat: 0.0,
      address_street: "",
      address_number:"",
      address_postal_code:"",
      address_city:"",
      address_country: "",
      login: "",
      password: "",
      second_password: "",
      nameRules: [
        v => !!v || "Name is required",
      ],
      streetRules: [
        v => !!v || "Street is required"
      ],
      numberRules: [
        v => !!v || "Number is required"
      ],
      codeRules: [
        //v => !!v || "Postal code is required"
      ],
      cityRules: [
        v => !!v || "City is required"
      ],
      countryRules: [
        v => !!v || "Country is required"
      ],
      loginRules: [
        v => !!v || "Login is required"
      ],
      passwordRules: [
        v => !!v || "Password is required"
      ],
      secondPasswordRules: [
        v => !!v || "Password is required",
        () => (this.second_password == this.password) || "Confirmation should be the same as password"
      ]
      
    };
  },
  
  methods: {
    validate () {
      if (this.$refs.form.validate()) {
        this.snackbar = true
      }
    },
    clearInput() {
      this.$refs.form.reset()
    },
    register(){
      if(this.$refs.form.validate()){
        if(this.password != this.second_password){
          //this.$refs.form.reset()
        }else{
          this.loading = true;
          if(this.owner){
            this.geocode();
          }else{
            this.registerUser();
          }
          
        }
        
      }
    },
    geocode() {
       axios.get(
        'https://maps.googleapis.com/maps/api/geocode/json?' + 'address='+ this.address_street + this.address_number + this.address_city + this.address_postal_code + this.address_country + '&key=AIzaSyCc1phOdSc1-2-v_mLiNV9Fb4u6lvlPG3k' 
        ).then(response => {
          console.log(response)
          this.lat = response.data.results[0].geometry.location.lat;
          this.lng = response.data.results[0].geometry.location.lng;
          this.registerRestaurant();
        }, error => {
          console.log(error);
        });
    },
    registerUser(){
        axios.post('http://192.168.1.15:9500/api/user/user/', JSON.stringify({
          email: this.login,
          password: this.password
        }),{
          headers: {
            'Content-Type': 'application/json'
          }
        }).then(() => {
          this.loading = false;
          this.$router.push('map');
        },error => {
          this.dialog = false;
          this.loading = false;
          console.log(error);
        })
    },
    registerRestaurant(){
        axios.post('http://192.168.1.15:9500/api/restaurant/restaurant', JSON.stringify({
          name: this.name,
          lng: this.lng,
          lat: this.lat,
        }),{
          headers: {
            'Content-Type': 'application/json'
          }
        }).then(() => {
          this.registerOwner();
        },error => {
          this.dialog = false;
          console.log(error);
        })
    },
    registerOwner(){
      
      axios.post('http://192.168.1.15:9500/api/user/user/', JSON.stringify({
          email: this.login,
          password: this.password,
          restaurant: {
            lng: this.lng,
            lat: this.lat
          }
        }),{
          headers: {
            'Content-Type': 'application/json'
          }
        }).then((response) => {
            console.log(response);
            this.loading = false;
          },error => {
          this.dialog = false;
          this.loading = false;
          console.log(error);
        })
        
      
    }
  }
};
</script>

<style>
</style>