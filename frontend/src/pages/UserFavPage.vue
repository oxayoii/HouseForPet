<template>
    <Menu></Menu>
    <div class="user-fav-container">
    <ItemUserFav  v-for="item in favoriteItems" :key="item.id" :item="item" class="itemUserFav" @delete-item="deleteItem" ></ItemUserFav>
  </div>
</template>
<script>
import Menu from '@/components/Menu.vue'
import ItemUserFav from '@/components/ItemUserFav.vue';
import { deleteFav, getUserFav } from '@/services/apiFav';

  export default {
  name: 'UserFav',
  components: {
    Menu,
    ItemUserFav
  },
  data() {
        return {
            favoriteItems: [], 
        };
    },
    async mounted() {
        await this.loadFavoriteItems();
    },
    methods: {
        async loadFavoriteItems() {
            try {
                const data = await getUserFav();
                if (data) {
                    this.favoriteItems = data.favDto;
                }
            } catch (error) {
                console.error('Ошибка при загрузке избранного:', error);
            }
        },
        async deleteItem(itemId) {
            try {
                await deleteFav(itemId); 
                await this.loadFavoriteItems(); 
            } catch (error) {
                console.error('Ошибка при удалении элемента:', error);
            }
        },
    },
}
</script>
<style>
.user-fav-container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 24px; /* расстояние между карточками */
  margin-top: 2%;
  padding: 16px;
  box-sizing: border-box;
}
.itemUserFav {
  display: flex;
  flex-wrap: wrap;
  gap: 24px; /* расстояние между карточками */
  margin-top: 2%;
  justify-content: center;
  box-sizing: border-box;
}
@media (max-width: 768px) {
    .user-fav-container {
        flex-direction: column; 
    }
    .itemUserFav {
        flex: 0 0 100%; 
    }
}
@media (max-width: 480px) {
    .user-fav-container {
        flex-direction: column; 
    }
    .itemUserFav {
        flex: 0 0 100%; 
        margin-top: 20%;
    }
}
</style>