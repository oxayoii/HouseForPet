<template>
  <div class="panelInfo">
    <div class="itemsSearch">

      <div class="form-group">
        <label for="gender" class="label">Пол:</label>
        <select v-model="gender" class="select">
          <option value="M">М</option>
          <option value="W">Ж</option>
        </select>
      </div>

      <div class="form-group">
        <label for="age" class="label">Возраст:</label>
        <select v-model="age" class="select">
          <option value="desc">По убыванию</option>
          <option value="inc">По возрастанию</option>
        </select>
      </div>
    </div>
    <div class="button-container">
      <button @click="searchPets" class="search-button">ПОКАЗАТЬ</button>
    </div>
  </div>
</template>
<script>
import {getAllPets} from '@/services/apiPets';

export default {
  data() {
    return {
      gender: '',
      age: ''
    };
  },
  methods: {
    async searchPets() {
      const searchParams = {
        SortGender: this.gender,
        SortOrder: this.age
      };
      try {
        const pets = await getAllPets(searchParams);
        this.$emit('pets-fetched', pets.petsDto);
      } catch (error) {
        console.error("Ошибка при поиске питомцев:", error);
      }
    }
  },
  mounted() {
    this.searchPets();
  }
}
</script>
<style>
.panelInfo{
  background: white;
  padding: 20px;
  border-radius: 20px;
}
.button-container{
  justify-content: space-around
}
.itemsSearch {
  display: flex;
  align-items: center;
}

.form-group {
  margin-left: 1%;
  margin-right: 1%;
  flex: 1;
}

.label {
  font-weight: bold;
  font-size: 15px;
  margin-right: 5%;
}

.select {
  flex: 1;
  font-size: 15px;
  width: 99%;
}

.search-button {
  background-color: white;
  margin-top: 4%;
  font-size: 2vh;
  width: 250px;
  height: 5vh;
  max-width: 200px;
  color: #331901;
  border: 2px solid black;
  border-radius: 10px;
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}

.search-button:hover {
  background-color: #331901;
  color: white;
}
</style>