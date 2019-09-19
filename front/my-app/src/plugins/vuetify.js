import Vue from 'vue';
import Vuetify from 'vuetify/lib';


Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    dark: false,
    themes:{
      dark:{
        primary: "#8bc34a",
        secondary: "#cddc39",
        accent: "#607d8b",
        error: "#f44336",
        warning: "#ffeb3b",
        info: "#03a9f4",
        success: "#4caf50",
        
        },
      light:
      {
      primary: "#8bc34a",
      secondary: "#cddc39",
      accent: "#607d8b",
      error: "#f44336",
      warning: "#ffeb3b",
      info: "#03a9f4",
      success: "#4caf50",
      background: "ff00ff"
      }
    }
  },
  icons: {
    iconfont: 'mdi',
  },
});
