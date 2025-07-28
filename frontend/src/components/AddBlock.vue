<template>
    <div class="add-item-panel">
        <h1 class="add-item-title">Добавьте данные:</h1>
        <input @change="onFileSelected" type="file" accept="image/*" ref="fileInput" class="add-item-input" />
        <input v-model="name" placeholder="Кличка" class="add-item-name" />
        <select v-model="gender" class="add-item-select">
            <option value="М">М</option>
            <option value="Ж">Ж</option>
        </select>
        <input v-model.number="age" placeholder="Возраст" class="add-item-age" />
        <textarea v-model="description" placeholder="Добавьте описание" class="add-item-description"></textarea>
        <button @click="addItem" class="add-item-button" :disabled="isAdding">  {{ isAdding ? 'Добавление...' : 'Добавить' }}</button>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    </div>
</template>
<script>
import { addImage, addPet } from '@/services/apiPets';

export default {
    data() {
    return {
      name: '',
      gender: 'М',
      age: null,
      description: '',
      imageFile: null,
      imageKey: null,  
      isAdding: false, 
      errorMessage: '',
    };
  },
  methods: {
    onFileSelected(event) {
      this.imageFile = event.target.files[0];
    },
    async addItem() {
      this.isAdding = true;
      this.errorMessage = '';
      const imageFile = this.imageFile
       if (!imageFile) {
        this.errorMessage = 'Выберите изображение.';
        this.isAdding = false;
        return; 
      }
      if(this.name.length < 2 || this.name.length > 15){
        this.errorMessage = 'Длина имени питомца должна составлять от 2 до 15 символов.'
        this.isAdding = false;
        return; 
      }
      if(!this.gender){
        this.errorMessage = 'Выберите пол потимца'
        this.isAdding = false;
        return; 
      }
      if (!this.age || !Number.isInteger(this.age)) {
           this.errorMessage = "Укажите корректный возраст животного";
           this.isAdding = false;
           return;
      }
      if(this.age > 15 || this.age < 1){
        this.errorMessage = "Возраст должен быть меньше 15 и больше 1";
        this.isAdding = false;
        return;
      } 
      if(this.description.length < 10 || this.description.length > 200){
        this.errorMessage = 'Длина описания должна составлять от 10 до 200 символов'
        this.isAdding = false;
        return; 
      }
      try {
        const imageKey = await addImage(imageFile);
        this.imageKey = imageKey;  

        const ResponsePets = {
            ImageUrl: imageKey,
            Name: this.name,
            Age: this.age,
            Gender: this.gender,
            Description: this.description
        }
        await addPet(ResponsePets);
        this.clearForm();
        this.$emit('update-pets'); 
      } catch (error) {
        this.errorMessage = error.response?.data?.message || 'Ошибка при добавлении питомца';
        console.error('Ошибка при добавлении питомца:', error);
      } finally {
        this.isAdding = false; 
      }
    },
      clearForm() {
      this.name = '';
      this.gender = 'M';
      this.age = null;
      this.description = '';
      this.imageFile = null;
      this.$refs.fileInput.value = null; 
      this.imageKey = null;
      }
    },
    emits: ['update-pets'],
}
</script>
<style>
.add-item-panel {
    background-color: #f9f9f9;
    border-radius: 25px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column;
    padding: 2%;
    max-width: 850px;
    margin: 3% auto;
}

.add-item-title {
    font-size: 3.3vw; 
    margin-bottom: 0.5em;
    text-align: center;
}

.add-item-input,
.add-item-name,
.add-item-age,
.add-item-description,
.add-item-select {
    margin: 10px 0;
    padding: 10px;
    font-size: 16px;
    border-radius: 5px;
    border: 1px solid #ccc;
    color: #331901;
}

.add-item-button {
    margin-top: 15px;
    background-color: white;
    color: black;
    font-size: 2vw; 
    border: 2px solid black; 
    border-radius: 10px;
    padding: 10px 20px;
    cursor: pointer;
    transition: background-color 0.3s;
    color: #331901;
    width: 100%; 
    
}

.add-item-button:hover {
    background-color: #331901; 
    color: white;  
}

@media (max-width: 768px) {
    .add-item-panel {
        max-width: 90%; 
        margin: 15% auto; 
    }
}

@media (max-width: 480px) {
    .add-item-title {
        font-size: 5vw; 
    }
    .add-item-button {
        width: 100%; 
        font-size: 3vw;
    }
}
</style>