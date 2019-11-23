<template>
  <v-app id="inspire">
    <v-navigation-drawer v-model="drawer" app clipped color="secondary">
      <v-list dense>

        <v-list-item to="/main/map">
          <v-list-item-action>
            <v-icon>mdi-map</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Map</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item to="/main/login">
          <v-list-item-action>
            <v-icon>mdi-login</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Login</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item to="/main/register">
          <v-list-item-action>
            <v-icon>mdi-account-plus</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Register</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item to="/main/menu" v-if="owner == true">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Menu</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

      </v-list>
    </v-navigation-drawer>

    <v-app-bar app clipped-left color="primary">
      <v-app-bar-nav-icon @click.stop="drawer = !drawer; update()"></v-app-bar-nav-icon>
      <v-toolbar-title>Application</v-toolbar-title>
      
      <v-btn color="secondary" absolute right @click="signOut()" v-if="isLogged">Sign Out</v-btn>
    </v-app-bar>

    <v-content>
      <v-container>
        <router-view @hook:destroyed="update()"/>
      </v-container>
    </v-content>

  </v-app>
</template>


<script>

export default {
  name: "MainPage",
  data: () => ({
    drawer: false,
    isLogged: false,
    owner: false,
    admin: false
  }),
  methods:{
    signOut(){
      localStorage.removeItem('token');
      localStorage.removeItem('restaurantId');
      this.owner = false;
      this.admin = false;
      this.isLogged = false;
      this.$router.push('map');
    },
    update(){
      if(localStorage.getItem('token') != null) {
        this.isLogged = true;
      }
      if(localStorage.getItem('restaurantId') > 0){
        this.owner = true;
      }
      else if(localStorage.getItem('restaurantId') == null){
        this.owner = false;
      }
    }
  }
};
</script>
