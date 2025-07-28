<template>
    <button @click="exit" class="butEsc">Выйти</button>
    <div class="itemPets">
      <AddBlock @update-pets="fetchPets"></AddBlock>
      <SearchBlock @pets-fetched="updatePets"></SearchBlock>
      <ItemPetAdmin @pet-updated="fetchPets" @pet-deleted="handlePetDeleted" v-for="pet in pets" :key="pet.id" :pet="pet"></ItemPetAdmin>
    </div>
</template>
<script>
import ItemPetAdmin from '@/components/ItemPetAdmin.vue';
import SearchBlock from '@/components/SearchBlock.vue';
import AddBlock from '@/components/AddBlock.vue';
import { logoutUser } from '@/services/apiUser';
import { getAllPets } from '@/services/apiPets';

export default {
  name: 'Admin',
  components: {
    ItemPetAdmin,
    SearchBlock,
    AddBlock
  },
  data() {
    return {
      pets: [] 
    };
  },
  mounted() {
    this.fetchPets(); 
  },
  methods:{
    async exit(){
      try {
        await logoutUser()
        this.$router.push('/avt')
      }
      catch (e) {
        this.errorMessage = e.response?.data?.message || "Произошла ошибка.";  
      }
    },
    async fetchPets() { 
        try {
            const response = await getAllPets();
            this.pets = response.petsDto; 
        } catch (error) {
            this.errorMessage = error.response?.data?.message || 'Ошибка при загрузке питомцев.';
            console.error('Ошибка при получении списка питомцев:', error);
        }
    },
    updatePets(pets) {
      this.pets = pets; 
    },
    handlePetDeleted(petId) {
      this.pets = this.pets.filter(pet => pet.id !== petId);
    },
  }
}
</script>
<style>
.itemPets{
    display: flex;
    flex-direction: column;
    justify-content: center; 
    align-items: center; 
}
.butEsc  {
    background-color: #ffffff; 
    color: rgb(0, 0, 0); 
    padding:0.7% 3%;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    border: none;
    border-radius: 15px; 
    cursor: pointer;
    font-size: 1em;
    margin-left: 2%;
    margin-top: 1%;
    color: #331901;
}
.butEsc:hover{
    background-color: #331901; 
    color: white; 
}
@media (max-width: 768px) {
    .butEsc{
      padding:1% 4%;
    }
}
@media (max-width: 480px) {
    .butEsc{
      padding:1.5% 4%;
    }
}
</style>