<template>
  <v-app id="inspire">
    <v-navigation-drawer v-model="drawer" app clipped color="secondary">
      <v-list dense>


        <v-list-item >
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Login</v-list-item-title>
          </v-list-item-content>
        </v-list-item>


        <v-list-item to="/main/map">
          <v-list-item-action>
            <v-icon>mdi-settings</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Map</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item to="/main/jokes">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Jokes</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

      </v-list>
    </v-navigation-drawer>

    <v-app-bar app clipped-left color="primary">
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title>Application</v-toolbar-title>
      <v-btn color="secondary" absolute right @click="signOut()">Sign Out</v-btn>
    </v-app-bar>

    <v-content>
      <v-container>
        <router-view />
      </v-container>
    </v-content>

  </v-app>
</template>


<script>

export default {
  name: "MainPage",
  data: () => ({
    drawer: null
  }),
  methods:{
    signOut: function(){
      this.$router.push('/');
    }
  }
};
</script>
