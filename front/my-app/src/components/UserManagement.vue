<template>
  <v-app>
    <v-data-table :headers="headers" :items="users">
    <template v-slot:item.action="{ item }">
      <v-btn v-if="item.isActive == 'true'" @click="blockUser(item)">Block</v-btn>
      <v-btn v-else @click="blockUser(item)">Unblock</v-btn>
    </template>
    </v-data-table>
  </v-app>
</template>

<script>
import axios from 'axios';
export default {
    data(){
        return{
            headers: [
            {
                text: 'User',
                align: 'left',
                sortable: false,
                value: 'id'
            },
            {text: 'Email', value: 'email'},
            {text: 'Admin?', value: 'isAdmin' },
            {text: 'Active?', value: 'isActive' },
            {text: 'Restaurant Id', value: 'restaurantId' },
            {text: 'Block User', value: 'action', sortable: false}
        ],
            users: []
        }
    },
    created() {
      this.getUsers();
    },
    methods: {
        getUsers(){
          const responseUsers = [];
            axios.get("http://localhost:9500/api/user/user",{
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                }
            })
            .then(response => {
              response.data.forEach(element => {
                responseUsers.push({
                  id: element.id,
                  email: element.email,
                  isAdmin: element.isAdmin.toString(),
                  isActive: element.isActive.toString(),
                  restaurantId: element.restaurantId
                })
              });
                this.users = responseUsers;

            }, error => {
                this.$router.push('map');
                console.log(error)
            });
        },
        blockUser(user){
          axios.put('http://localhost:9500/api/user/User/'+ user.id +'/Block',{},{
            headers: {
              'Authorization': 'Bearer ' + localStorage.getItem('token')
            }}
          ).then(() => {
              this.getUsers()
            }, error => {
              console.log(error);
            })
        }
    }
}
</script>