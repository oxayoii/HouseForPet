<template>
  <Menu></Menu>
  <div class="main-container">
    <InfoBlock></InfoBlock>
    <label class="searchHouse">ИЩУТ ДОМ</label>
    <SearchBlock @pets-fetched="updatePets"></SearchBlock>
    <div class = "pets-panel">
      <ItemBlock v-for="pet in pets" :key="pet.id" :pet="pet"></ItemBlock>
    </div>
  </div>
</template>
<script>
  import Menu from '@/components/Menu.vue'
  import InfoBlock from '@/components/InfoBlock.vue';
  import SearchBlock from '@/components/SearchBlock.vue';
  import ItemBlock from '@/components/ItemBlock.vue';

  export default {
  name: 'App',
  components: {
    Menu,
    InfoBlock,
    SearchBlock,
    ItemBlock
  },
  data() {
      return {
          pets: [] 
      };
  },
  methods: {
     updatePets(pets) {
      this.pets = pets; 
    }
  }
}
</script>

<style scoped>
.pets-panel {
  display: flex;
  flex-wrap: wrap;
  gap: 24px; /* расстояние между карточками */
  margin-top: 2%;
  justify-content: center;
  padding: 16px;
  box-sizing: border-box;
}

/* Стили для ItemBlock (если не определены внутри самого ItemBlock) */
.item-card {
  border: 1px solid #ddd;
  border-radius: 16px;
  background-color: white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  transition: box-shadow 0.3s;
  width: 320px;
  flex-shrink: 0;
}

.item-card:hover {
  box-shadow: 0 6px 18px rgba(0, 0, 0, 0.1);
}

.searchHouse {
  font-size: 70px;
  margin-top: 10%;
  font-weight: bold;
  text-align: center;
  color: #331901;
}

.main-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin-top: 7%;
}

@media (max-width: 768px) {
  .item-card {
    width: 90%;
  }
}

@media (max-width: 400px) {
  .searchHouse {
    font-size: 50px;
  }
}

</style>