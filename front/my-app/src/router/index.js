import Vue from 'vue';
import Router from 'vue-router';

import MainPage from '../components/MainPage';
import LoginPage from '../components/LoginPage';
import MapComponent from '../components/MapComponent';
import RegisterForm from '../components/RegisterForm';
import MenuEditComponent from '../components/MenuEditComponent';
import UserManagement from '../components/UserManagement';

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/main',
            component: MainPage,
            children: [
                {
                    path: 'map',
                    component: MapComponent
                },
                {
                    path: 'login',
                    component: LoginPage
                },
                {
                    path: 'register',
                    component: RegisterForm
                },
                {
                    path: 'menu',
                    component: MenuEditComponent
                },{
                    path: 'admin',
                    component: UserManagement
                }
            ]
        },
        {
            path: '/',
            redirect: '/main/map'
        }
    ]
})