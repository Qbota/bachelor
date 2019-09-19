<template>
  <v-app>
    <v-content class="pa-0">
      <v-container>
        <v-row>
          <v-col cols="2">
            <div class="flex-grow-1"></div>
            <v-btn @click="getJokes()" color="primary">Click here!</v-btn>
          </v-col>
          <v-col cols="10">
            <v-list two-line="true">
              <v-subheader>Downloaded jokes</v-subheader>
              <v-list-item-group v-model="item">
                <v-list-item v-for="(item,i) in this.jokes" :key="i">
                  <v-list-item-content>
                    <v-list-item-title v-text="item.id"></v-list-item-title>
                    <v-list-item-subtitle v-text="item.joke"></v-list-item-subtitle>
                  </v-list-item-content>
                </v-list-item>
              </v-list-item-group>
            </v-list>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
import axios from 'axios';
export default {
    name: "Jokes",
    props: {
        jokes: []
    },
    methods: {
    getJokes: function(){
            axios.get("http://api.icndb.com/jokes/random/10")
            .then((response) => {
                this.jokes = response.data.value;
                console.log(response);
            }, (error) => {
                console.log(error);
            })
        }
    
  }
}
</script>