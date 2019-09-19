import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import router from './router/index';
import * as VueGoogleMaps from 'vue2-google-maps';

Vue.config.productionTip = false

Vue.use(VueGoogleMaps,{
  load: {
    key: "AIzaSyA8VWPtO89SIfwked_TRedJgLfck0gvNlc",
    libraries: "places"
  }
})

new Vue({
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')
