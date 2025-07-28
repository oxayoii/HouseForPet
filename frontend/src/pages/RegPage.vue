<template>
  <button @click="goToMainPage" class="buttonEsc2">Вернуться</button>
    <div class="panelReg2">
    <RegBlock/>
    <button @click="goToAvtPage" class="buttonGoAvt">Уже есть аккаунт</button>
    </div>
</template>
<script>
export default {
    name: 'Reg',
    components: {
    RegBlock
  },
  methods: {
        goToMainPage(){
            this.$router.push('/')
        },
        goToAvtPage(){
            this.$router.push('/avt')
        }
    }
}
import RegBlock from '@/components/RegBlock.vue'
</script>
<style>
.panelReg2{
  align-items: center; 
  display: flex;
  flex-direction: column; 
}
.buttonGoAvt{
    background-color: #ffffff; 
    color: #331901; 
    padding:0.7% 7%;
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
.buttonEsc2 {
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
.buttonEsc2:hover {
    background-color: #331901;
    color: white;
}
@media (max-width: 768px) {
  .buttonEsc2{
    padding:1% 3%;
  }
}
@media (max-width: 480px) {
  .buttonEsc2{
    padding:2% 3%;
  }
}
</style>