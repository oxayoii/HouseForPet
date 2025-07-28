<template>
     <div class="item-fav-panel">
        <div class="item-fav-image-container">
            <img :src="item.imageUrl" alt="Изображение питомца" class="itemFav-dog-image"/>
        </div>
        <div class="item-fav-content">
            <h1>{{ item.name }}</h1>
            <p class="item-age">{{ item.gender == 'M' ? 'Мальчик' : 'Девочка' }}, {{ formatAge(item.age) }}</p>
            <p class="item-description">{{ item.description }}</p>
            <button class="butDelete" @click="deleteItem">Удалить</button>
        </div>
    </div>
 </template>
 <script>
 export default {
     name: 'ItemUserFav',
     props: {
         item: {
             type: Object,
             required: true,
         },
     },
     methods: {
        deleteItem() {
            this.$emit('delete-item', this.item.id);  
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
    }
    },
 };
</script>
 <style>
.item-fav-panel {
   display: flex;
   flex-direction: column;
   border-radius: 16px;
   box-shadow: 0 6px 18px rgba(0, 0, 0, 0.1);
   background-color: white;
   max-width: 360px;
   margin: auto;
   transition: transform 0.3s;
}
.item-fav-image-container {
  position: relative;
  width: 100%;
  aspect-ratio: 4/4;
  overflow: hidden;
}
.itemFav-dog-image {
    width: 100%;
    border-radius: 25px;
}

.item-fav-content{
    padding: 16px;
    flex: 2;
    margin-left: 2%;
}

.item-type, .item-age {
    font-size: 18px;
    margin: 5px 0;
    color: #666;
}

.item-description {
    font-size: 16px;
    color: #333;
}

.butDelete {
    background-color: white;
    font-size: 2.3vh;
    width: 10vw;
    height: 6vh;
    color: black;
    border: 2px solid black;
    border-radius: 20px;
    cursor: pointer;
    transition: background-color 0.3s, color 0.3s;
}

.butDelete:hover {
    background-color: black;
    color: white;
}

@media (max-width: 768px) {
    .butDelete{
        width: 25vw;
    }
}

@media (max-width: 480px) {
    .butDelete{
        width: 25vw;
    }
}
</style>