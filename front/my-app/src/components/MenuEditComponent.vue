<template>
  <v-app>
      <v-data-table
    :headers="headers"
    :items="menu"
    :search="search"
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar flat color="white">
        <v-toolbar-title>Menu</v-toolbar-title>
        <v-spacer/>
        <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
        <v-divider
          class="mx-4"
          inset
          vertical
        ></v-divider>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" max-width="500px">
          <template v-slot:activator="{ on }">
            <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.name" label="Meal Name"></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field v-model="editedItem.price" label="Price"></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="save">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item.action="{ item }">
      <v-icon
        small
        class="mr-2"
        @click="editItem(item)"
      >
        mdi-square-edit-outline
      </v-icon>
      <v-icon
        small
        @click="deleteItem(item)"
      >
        mdi-delete
      </v-icon>
    </template>
    <template v-slot:no-data>
      <v-btn color="primary" @click="getMenu">Reset</v-btn>
    </template>
  </v-data-table>
    <v-btn color="primary" @click="saveMenu">Save Changes</v-btn>
    <v-snackbar
      v-model="saved"
    >
      Saved
      <v-btn
        color="pink"
        text
        @click="saved = false"
      >
        Close
      </v-btn>
    </v-snackbar>
    <v-snackbar
      v-model="errorSaved"
    >
      Error
      <v-btn
        color="pink"
        text
        @click="errorSaved = false"
      >
        Close
      </v-btn>
    </v-snackbar>
  </v-app>
</template>

<script>
import axios from 'axios';
export default {
  name: 'MenuEditComponent',
  data() {
    return {
      dialog: false,
      saved: false,
      errorSaved: false,
      search: '',
      editedIndex: -1,
      editedItem: {
        name: '',
        comments: null,
        averageRate: 0,
        price: 0,
        restaurantId: 0
      },
      defaultItem: {
        name: '',
        comments: null,
        averageRate: 0,
        price: 0,
        restaurantId: 0
      },
        loading: true,
        menu: [],
        headers: [
            {
                text: 'Menu item',
                align: 'left',
                sortable: false,
                value: 'name'
            },
            {text: 'Price', value: 'price'},
            {text: 'Average Rate', value: 'averageRate'},
            { text: 'Actions', value: 'action', sortable: false }
        ]
    }
  },

  computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
    },

    created () {
      this.getMenu()
    },
  methods: {
      editItem (item) {
        this.editedIndex = this.menu.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        const index = this.menu.indexOf(item)
        confirm('Are you sure you want to delete this item?') && this.menu.splice(index, 1)
      },

      close () {
        this.dialog = false
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },

      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.menu[this.editedIndex], this.editedItem)
        } else {
          this.menu.push(this.editedItem)
        }
        this.close()
      },
      saveMenu(){
        let content = [];
        this.menu.forEach(element => {
          content.push({
            id: 0,
            name: element.name,
            averageRate: element.averageRate,
            price: element.price,
            restaurantId: this.restaurantId
          })
        });
        console.log(content);
        axios.put("https://localhost:44340/api/restaurant/restaurant/menu", JSON.stringify(content),
        {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
          }
        })
        .then(() => {
          this.saved = true;
        }, error => {
          console.log(error);
          this.errorSaved = true;
        });
      },
    getMenu() {
      const response_menu = [];
      console.log('Bearer' + localStorage.getItem('token'))
      //if menu is null restaurant id is not assigned
      axios.get("https://localhost:44340/api/restaurant/restaurant/menu",{
          headers: {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
          }
        })
      .then(response => {
        this.restaurantId = localStorage.getItem('restaurantId')
        response.data.forEach(element => {
          response_menu.push({
            name: element.name,
            averageRate: element.averageRate,
            price: element.price,
            restaurantId: this.restaurantId
          })
        });
        
      }, error => {
        this.$router.push('map');
        console.log(error)
      });
      
      this.menu = response_menu;
      this.loading = false;
    },
  }
};
</script>

<style>
</style>