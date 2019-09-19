import Vue from 'vue';
import Router from 'vue-router';

import MainPage from '../components/MainPage';
import LoginPage from '../components/LoginPage';
import Jokes from '../components/Jokes';
import MapComponent from '../components/MapComponent';

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/main',
            component: MainPage,
            children: [
                {
                    path: 'jokes',
                    component: Jokes
                },
                {
                    path: 'map',
                    component: MapComponent
                }
            ]
        },
        {
            path: '/login',
            component: LoginPage
        },
        {
            path: '/',
            redirect: '/login'
        }
    ]
})