<template>
  <div v-if="isOpen" class="modal-overlay" @click.self="closeModal">
      <div class="add-item-panel">
          <h1 class="add-item-title">Редактировать данные:</h1>
          <img class="image" v-if="pet.imageUrl" :src="pet.imageUrl" alt="Текущее изображение" style="max-width:  150px; max-height: auto">
          <input type="file" accept="image/*" class="add-item-input" @change="onFileSelected" ref="fileInput" />
          <input placeholder="Кличка" class="add-item-name" v-model="pet.name" />
          <select v-model="pet.gender" class="add-item-select">
              <option value="М">М</option>
              <option value="Ж">Ж</option>
          </select>
          <input placeholder="Возраст" class="add-item-age" v-model="pet.age" />
          <textarea placeholder="Добавьте описание" class="add-item-description" v-model="pet.description"/>
          <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
          <button class="add-item-button" @click="saveChanges">Сохранить</button>
          <button class="add-item-button" @click="closeModal">Закрыть</button>
      </div>
  </div>
</template>

<script>
import { updateImage, updatePet } from '@/services/apiPets';

export default {
  props: {
      pet: {
          type: Object,
          required: true,
      },
  },
  data() {
      return {
          isOpen: false,
          imageFile: null,
          errorMessage: '',
      };
  },
  mounted() { 
      console.log('EditModal mounted with pet:', this.pet);
  },
  methods: {
      openModal() { 
          this.isOpen = true;
      },
      closeModal() {
          this.isOpen = false;
          this.resetForm();
      },
      onFileSelected(event) {
          this.imageFile = event.target.files[0];
      },
      async saveChanges() {
          try {
            let newImageUrl = null;
            let currentImageKey = this.pet.imageUrl.split('/').pop();
            if (this.imageFile) {
                    const newImageKey = await updateImage(this.imageFile, currentImageKey);
                    newImageUrl = newImageKey;
            } 
            else {
                    newImageUrl = currentImageKey; 
            }
            if(this.pet.name.length < 2 || this.pet.name.length > 15){
                    this.errorMessage = 'Длина имени питомца должна составлять от 2 до 15 символов.'
                    this.isAdding = false;
                    return; 
            }
            if(!this.pet.gender){
                    this.errorMessage = 'Выберите пол потимца'
                    this.isAdding = false;
                    return; 
            }
            if (!this.pet.age || !Number.isInteger(Number(this.pet.age))) {
                    this.errorMessage = "Укажите корректный возраст животного";
                    this.isAdding = false;
                    return;
            } 
            if(this.pet.age > 15 || this.pet.age < 1){
                    this.errorMessage = "Возраст должен быть меньше 15 и больше 1";
                    this.isAdding = false;
                    return;
            } 
            if(this.pet.description.length < 10 || this.pet.description.length > 200){
                    this.errorMessage = 'Длина описания должна составлять от 10 до 200 символов'
                    this.isAdding = false;
                    return; 
            }
            const petData = {
                  ImageUrl: newImageUrl || this.pet.imageUrl,
                  Name: this.pet.name,
                  Age: Number(this.pet.age),
                  Gender: this.pet.gender,
                  Description: this.pet.description,
            };
            await updatePet(this.pet.id, petData);
            this.$emit('pet-updated');
            this.closeModal();
          } catch (error) {
             // console.error('Ошибка при сохранении изменений:', error);
              this.errorMessage = error.response?.data?.message || "Произошла ошибка.";  
          }
      },
      resetForm() {
          this.imageFile = null;
          if (this.$refs.fileInput) {
              this.$refs.fileInput.value = null;
          }
      },
  },
};
</script>
<style>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5); /* Полупрозрачный фон */
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000; /* Убедитесь, что он выше всего остального */
  overflow: hidden; /*  Включаем прокрутку на оверлее */
}
.add-item-panel {
  background-color: white;
  border-radius: 10px;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.2);
  max-width: 80%;
  max-height: 100%;
  overflow: auto; 
  padding: 20px;
  display: flex;
  flex-direction: column;
}
.image {
  max-width: 200px; /*  Изменили значение */
  max-height: 200px; /*  Изменили значение */
  width: auto; /*  Сохраняем пропорции */
  height: auto; /* Сохраняем пропорции */
  border-radius: 10px; /* Добавил скругление */
  object-fit: cover; /* Масштабируем изображение с обрезкой */
  display: block; /* Превращаем в блочный элемент */
  margin-left: auto; /* Центрируем по горизонтали */
  margin-right: auto; 
}
</style>