<template>
  <div class="ItempanelAdmin">
      <div class="ItemAdmin-image-container">
          <img :src="pet.imageUrl" class="itemAdmin-dog-image" />
      </div>
      <div class="itemAdmin-content">
          <div class="itemAdmin-header">
              <h1>{{ pet.name }}</h1>
          </div>
          <p>{{ pet.gender == 'M' ? 'Мальчик' : 'Девочка' }}, {{ formatAge(pet.age) }}</p>
          <p>{{ pet.description }}</p>
          <button @click="openEditModal" class="buttonManage">Редактировать</button>
          <EditModal ref="modal" @pet-updated="handlePetUpdated" :pet="pet" />  
          <button @click="confirmDelete" class="buttonManage">Удалить</button>
          <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
      </div>
  </div>
</template>

<script>
import { deleteImage, deletePet } from '@/services/apiPets';
import EditModal from './EditModal.vue';

export default {
  data() {
    return {
      errorMessage: null, 
    };
  },
  components: {
      EditModal,
  },
  props: {
      pet: {
          type: Object,
          required: true,
      },
  },
  methods: {
      async confirmDelete() {
          if (confirm('Вы уверены, что хотите удалить этого питомца?')) {
              await this.deletePet();
          }
      },
      async deletePet() {
          try {
                if (this.pet.imageUrl) {
                    const imageKey = this.pet.imageUrl.split('/').pop();
                    if (imageKey) {
                       await deleteImage(imageKey);
                    }
                } 
                await deletePet(this.pet.id);
                this.$emit('pet-deleted', this.pet.id);
          } catch (error) {
           //   console.error('Ошибка при удалении питомца:', error);
              this.errorMessage = error.response?.data?.message || 'Ошибка при удалении питомца'; 
          }
      },
      openEditModal() {
          console.log('Pet being passed to modal:', this.pet);
          this.$refs.modal.openModal(); 
      },
      handlePetUpdated() {
          this.$emit('pet-updated');
      },
      formatAge(age) {
      if (age === null || age === undefined) {
        return "Возраст не указан"; 
      }

      if (age > 4) {
        return `${age} лет`;
      }
      else if(age == 1) {
        return `${age} год`;
      }
      else {
        return `${age} года`;
      }
    },
  },
};
</script>
<style>
.ItempanelAdmin {
    background-color: #f9f9f9;
    border-radius: 45px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    display: flex;
    padding: 2%;
    width: 850px; 
    height: auto;
    margin-top: 5%;
}

.ItemAdmin-image-container {
    flex: 1;
}

.itemAdmin-dog-image {
    border-radius: 45px;
    width: 100%;
}

.itemAdmin-content {
    flex: 2;
    margin-left: 2%;
}

h1 {
    font-size: 2em;
    margin-bottom: 1%;
    margin-top: 0.5%;
}

p {
    font-size: 1.2em;
    line-height: 1.5;
}

.buttonManage {
    background-color: white;
    margin-top: 1%;
    font-size: 2.4vh;
    width: 20vw;
    height: 6vh;
    color: #331901;
    border: 2px solid black;
    border-radius: 10px;
    margin-right: 2%;
    cursor: pointer;
    transition: background-color 0.3s, color 0.3s;
}

.buttonManage:hover {
    background-color: #331901;
    color: white;
}
@media (max-width:900px) {
  .ItempanelAdmin {
    background-color: #f9f9f9;
    border-radius: 45px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    display: flex;
    padding: 2%;
    width: 720px; 
    height: auto;
    margin-top: 5%;
}
}
@media (max-width: 768px) {
    .buttonManage {
        width: 30vw;
        font-size: 2vh;
    }

    .ItempanelAdmin {
        flex-direction: column;
        width: 80%;
        margin: 20% auto;
    }

    .itemAdmin-content {
        margin-left: 0;
        text-align: center; /* Центрируем текст внутри контента */
    }

    h1 {
        font-size: 1.5em;
    }

    p {
        font-size: 1em;
    }

    .itemAdmin-header {
      justify-content: center; /*  Если header flex, то центрируем элементы */
      align-items: center;
        text-align: center; /* Центрируем заголовок */
    }
}

@media (max-width: 480px) {
    .buttonManage {
        width: 30vw;
        font-size: 1.9vh;
    }

    .ItempanelAdmin {
        flex-direction: column;
        width: 80%;
        margin: 20% auto;
    }

    h1 {
        font-size: 1.2em;
    }

    .itemAdmin-dog-image {
        width: 100%;
        border-radius: 45px;
    }

    p {
        font-size: 0.9em;
    }

    .itemAdmin-header {
        justify-content: center; /* Если flex то центрируем элементы внутри header */
        align-items: center;
        text-align: center; /* Центрируем заголовок */
    }
}
</style>