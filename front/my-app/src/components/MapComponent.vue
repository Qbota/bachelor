<template>
    <v-app>
  <v-card
    color="grey lighten-4"
    flat
    tile
  >
   <v-toolbar absolute floating>
      <v-toolbar-title class="px-4">Search</v-toolbar-title>
      <v-toolbar-items>
      <v-autocomplete
      class="px-4"
        v-model="searchInput"
        hint="Search something !"
        :items="meals"
        :filter="filter"
        aria-autocomplete="off"
        @keyup.enter="getMarkers()"
      ></v-autocomplete>

      <v-btn icon @click="geolocate()" class="px-4">
        <v-icon>mdi-crosshairs-gps</v-icon>
      </v-btn>

      <v-btn icon @click="getMarkers()" class="px-4">
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
      </v-toolbar-items>
    </v-toolbar>
  </v-card>

<v-dialog v-model="dialog" max-width="30%" class="text-center">
  <v-card>
    <v-card-title>
      Information or Rating?
      <v-spacer/>
      <v-switch v-model="dialogSwitch"></v-switch>
    </v-card-title>
    <v-card-text v-if="dialogSwitch == true">
      Rate This Meal!
      <v-rating v-model="rating"></v-rating>
      <v-text-field v-model="commentText" placeholder="Insert comment text here" @keydown.enter="sendComment()"></v-text-field>
    </v-card-text>
    <v-card-actions v-if="dialogSwitch == true">
        <v-spacer/>
        <v-btn @click="sendComment()" >Rate</v-btn>
        <v-btn @click="closeDialog()">Close</v-btn>
    </v-card-actions>
    <v-card-text v-if="dialogSwitch == false">
        Chosen meal info here!
        <v-text-field label="Name" v-model="chosenMeal.name" readonly/>
        <v-text-field label="Price" v-model="chosenMeal.price" readonly suffix="Zł"/>
        <v-text-field label="Average Rating" v-model="chosenMeal.averageRate"  readonly/>
        <v-list class="overflow-y-auto" max-height="300">
          <v-subheader>Comments</v-subheader>
          <v-list-item-group>
            <v-list-item
              v-for="(item, i) in chosenMeal.comments"
              :key="i"
            >
            <v-list-item-icon>
              <v-icon>mdi-comment</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title v-text="item"/>
            </v-list-item-content>
            <v-list-item-action>
              <v-text-field v-text="chosenMeal.rates[i]"/>
            </v-list-item-action>
            
            </v-list-item>
          </v-list-item-group>
        </v-list>
    </v-card-text>
    <v-card-actions v-if="dialogSwitch == false">
      <v-spacer/>
      <v-btn @click="closeDialog()">Close</v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>

    <gmap-map
      :center="center"
      :zoom="12"
      style="width:100%;  height: 80%;"
    >

      <gmap-marker
        :key="index"
        v-for="(m, index) in markers"
        :position="m.position"
        @click="showDialog(m.mealInfo)"
      />
      <!-- @click="openInfoWindow(m, index)" -->
    </gmap-map>
    
    </v-app>
</template>

<script>
import axios from 'axios';

export default {
    name: "MapComponent",
    data() {
    return {
      center: { lng: 17.034303, lat: 51.110440 },
      markers: [],
      places: [],
      currentPlace: null,
      showInfoWindow: false,
      infoWindowPosition: null,
      currentInfoWindowIdx: null,
      searchInput: null,
      infoOptions: {
        content: '',
          //optional: offset infowindow so it visually sits nicely on top of our marker
          pixelOffset: {
            width: 0,
            height: -35
            }
      },
      meals: [],
      dialog: false,
      rating: 0,
      commentText: '',
      chosenMeal: {
        id: 0,
        name: '',
        comments: '',
        rates: '',
        averageRate: 0.0,
        price: 0.0
      },
      dialogSwitch: false
    }
    },
    mounted(){
      //this will set center of the map on user location
      //this.geolocate();
      this.getMeals();
    },
    methods: {
      sendComment(){
        let comment = {
          rate: this.rating,
          text: this.commentText,
          mealid: this.chosenMeal.id,
        };
        axios.post('https://localhost:44340/api/restaurant/meal/comment', JSON.stringify(comment),{
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
          }
        }).then(error => {
          this.dialog = false;
          console.log(error);
        })
        this.closeDialog();
      },
      showDialog(mealInfo){
        this.chosenMeal = mealInfo;
        this.dialog = true;
      },
      closeDialog(){
        this.chosenMeal = {
          id: 0,
          name: '',
          comments: '',
          rates: '',
          averageRate: 0.0,
          price: 0.0
        };
        this.dialog = false;
      },
      filter (item, queryText) {
        return queryText.length > 2 && item.toLowerCase().indexOf(queryText) > -1
      },
      getMeals(){
        axios.get("https://localhost:44340/api/restaurant/meal")
        .then(response => {
          response.data.forEach(element => {
            this.meals.push(element.name)
          });
        }, error => {
          console.log(error)
        });
      },
      geolocate() {
      navigator.geolocation.getCurrentPosition(position => {
        this.center = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };
      })
      },
      getMarkers(){
        this.markers = []
        let places = []
        //let res = axios.get('https://localhost:44340/api/restaurant/GetPlacesWithMeal/' + this.searchInput)
        axios.get('https://localhost:44340/api/restaurant/GetPlacesWithMeal/' + this.searchInput)
        .then((response) => {
            response.data.forEach(element => {
              places.push({
                position: {
                lng: element.lng,
                lat: element.lat
              },
              infoText: element.name,
              mealInfo: {
                id: element.menu[0].id,
                name: element.menu[0].name,
                comments: element.menu[0].comments,
                rates: element.menu[0].rates,
                averageRate: element.menu[0].averageRate,
                price: element.menu[0].price,

              }
              })
            })
        }, (error) => {
          console.log(error);
        });
        this.markers = places;
      },
    }
    
    
    
}
</script>