<template>
    <div class="Mainpanel">
    <div class="image-container">
      <img src="/src/assets/mainPhoto.png" alt="Собака" class="dog-image"/>
    </div>
    <div class="Maincontent">
      <h1>Приют для животных «Дом с хвостом»</h1>
      <p>Добро пожаловать в приют для животных в Балашихе!<br>
    Мы заботимся о бездомных животных 
    и помогаем им найти любящие семьи.<br>
    Свяжитесь с нами: 8-967-274-75-00.<br>
    Адрес: г. Балашиха.</p>
    </div>
  </div>
</template>
<style>
.Mainpanel {
  display: flex;
  padding: 2%;
  max-width: 1500px;
  height: auto;
  margin: 2% auto;
  margin-left: 7%;
}
.image-container {
  flex: 1;
}
.dog-image {
  width: 100%;
  height: 100%; 
}
.Maincontent {
  flex: 2;
  margin-top: 2%;
  font-size: 2.5vh;
}
h1 {
  font-size: 4vh;
  margin-bottom: 1%;
  margin-top: 0.5%;
}
p {
  font-size: 3vh;
  line-height: 1.5;
}
@media (max-width: 768px) {
    .Mainpanel {
        flex-direction: column; 
        margin: 20% auto;
    }
    .Maincontent {
        margin-left: 0; 
        text-align: center; 
    }
    h1 {
        font-size: 1.5em;
    }
    p {
        font-size: 1em; 
    }
}
@media (max-width: 480px) {
    h1 {
        font-size: 1.2em; 
    }
    .dog-image {
      width: 100%; 
      height: auto; 
    }
    p {
        font-size: 0.9em; 
    }
}
</style>