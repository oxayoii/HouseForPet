import { createRouter, createWebHistory } from 'vue-router'
import HomePage from './pages/MainPage.vue'
import Reg from './pages/RegPage.vue'
import Avt from './pages/AvtPage.vue'
import UserFav from './pages/UserFavPage.vue'
import AdminPage from './pages/AdminPage.vue'

const routes = [
    {
        path: '/',
        component: HomePage, 
    },
    {
        path: '/reg',
        component: Reg,
    },
    {
        path: '/avt',
        component: Avt,
    },
    {
        path: '/fav',
        component: UserFav
    },
    {
        path: '/admin',
        component: AdminPage,
        meta: {requiredAuth: true}
    }
]
const router = createRouter({
    history: createWebHistory(),
    routes,
})
router.beforeEach((to, from, next) => {
    if (to.path === '/avt' || to.path === '/reg') {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
    }
    const protectedPaths = ['/fav', '/admin'];
    if (protectedPaths.includes(to.path)) {
        const accessToken = localStorage.getItem('access_token');
        const refreshToken = localStorage.getItem('refresh_token');
        if (!accessToken || !refreshToken) {
            alert('Сначала войдите в приложение!');
            next(false);
            return;
        }
    }
    next();
});
export default router