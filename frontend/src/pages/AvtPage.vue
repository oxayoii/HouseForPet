<template>
    <button @click="goToMainPage" class="buttonEsc">Вернуться</button>
    <div class="panelReg2">
    <AvtBlock></AvtBlock>
    <button @click="goToRegPage" class="buttonGoAvt">У меня нет аккаунта</button>
    </div>
</template>
<script>
export default{
    components: {
        AvtBlock
    },
    methods: {
        goToMainPage(){
            this.$router.push('/')
        },
        goToRegPage(){
            this.$router.push('/reg')
        }
    }
}
import AvtBlock from '@/components/AvtBlock.vue';
</script>
<style>
.panelReg2{
  align-items: center; 
  display: flex;
  flex-direction: column; 
}
.buttonGoAvt{
    background-color: #ffffff; 
    color:  #331901; 
    padding:0.7% 5%;
    border: none;
    border-radius: 15px; 
    cursor: pointer;
    border: 1px solid #999999;
    font-size: 1em;
}
.buttonGoAvt:hover {
    background-color: #331901;
    color: white;
}
.buttonEsc {
    background-color: #ffffff; 
    color: #331901; 
    padding:0.7% 2%;
    border: none;
    border-radius: 15px; 
    cursor: pointer;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    font-size: 1em;
    margin-left: 2%;
    margin-top: 1%;
}
.buttonEsc:hover {
    background-color: #331901;
    color: white;
}
@media (max-width: 768px) {
  .buttonEsc{
    margin-left: 2%;
    padding:1% 3%;
  }
}
@media (max-width: 480px) {
  .buttonEsc{
    margin-left: 2%;
    padding:2% 3%;
  }
}
</style>