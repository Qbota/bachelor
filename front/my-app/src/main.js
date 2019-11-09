import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import router from './router/index';
import * as VueGoogleMaps from 'vue2-google-maps';

Vue.config.productionTip = false

Vue.use(VueGoogleMaps,{
  load: {
    key: "AIzaSyCc1phOdSc1-2-v_mLiNV9Fb4u6lvlPG3k",
    libraries: "places"
  }
})

new Vue({
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')
