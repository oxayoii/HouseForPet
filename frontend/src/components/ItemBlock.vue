<template>
  <div class="item-card">
    <div class="image-wrapper">
      <img :src="currentImage" @error="handleImageError" class="item-image" />
      <img class="fav-icon" src="/src/assets/heart.png" @click="addToFavorites" />
    </div>
    <div class="item-info">
      <h2 class="item-name">{{ pet.name }}</h2>
      <p class="item-sub">{{ pet.gender === 'M' ? 'Мальчик' : 'Девочка' }}, {{ formatAge(pet.age) }}</p>
      <p class="item-desc">{{ pet.description }}</p>
    </div>
  </div>
</template>


<script>
import { addFav } from '@/services/apiFav';

export default {
  name: 'ItemBlock',
  props: {
    pet: { type: Object, required: true }
  },
  data() {
    return {
      currentImage: this.pet.imageUrl || '/src/assets/photo.jpg'
    };
  },
  methods: {
    async addToFavorites() {
      try {
        await addFav(this.pet.id);
      } catch (error) {
        console.error('Ошибка при добавлении в избранное:', error);
      }
    },
    formatAge(age) {
      if (age == null) return 'Возраст не указан';
      if (age === 1) return '1 год';
      if (age > 4) return `${age} лет`;
      return `${age} года`;
    },
    handleImageError() {
      this.currentImage = '/src/assets/photo.jpg';
    }
  }
};


</script>
<style>
.item-card {
  display: flex;
  flex-direction: column;
  border-radius: 16px;
  box-shadow: 0 6px 18px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  background-color: white;
  max-width: 360px;
  margin: auto;
  transition: transform 0.3s;
}

.item-card:hover {
  transform: translateY(-5px);
}

.image-wrapper {
  position: relative;
  width: 100%;
  aspect-ratio: 4 / 3;
  overflow: hidden;
}

.item-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.fav-icon {
  position: absolute;
  top: 12px;
  right: 12px;
  width: 50px;
  height: 35px;
  cursor: pointer;
  filter: brightness(1);
  transition: transform 0.2s, filter 0.3s;
}

.fav-icon:hover {
  transform: scale(1.1);
  filter: brightness(0.8);
}

.item-info {
  padding: 16px;
}

.item-name {
  font-size: 1.6em;
  font-weight: bold;
  margin-bottom: 8px;
  color: #331901;
}

.item-sub {
  font-size: 1.1em;
  color: #666;
  margin-bottom: 12px;
}

.item-desc {
  font-size: 1em;
  color: #444;
  line-height: 1.5;
}

/* Адаптив */
@media (max-width: 768px) {
  .item-card {
    max-width: 90%;
  }

  .item-name {
    font-size: 1.4em;
  }

  .item-sub,
  .item-desc {
    font-size: 0.95em;
  }

  .fav-icon {
    width: 50px;
    height: 35px;
  }
}

</style>