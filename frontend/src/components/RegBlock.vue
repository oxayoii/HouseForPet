<template>
    <div class="panelReg">
        <h1 class="createAcc">СОЗДАЙТЕ АККАУНТ</h1>
        <input v-model="login" type="text" placeholder="Логин" class="inputField">
        <input v-model="password" type="password" placeholder="Пароль" class="inputField">
        <input  v-model="repeatPassword" type="password" placeholder="Повторите пароль" class="inputField">
        <button @click="registerUser" class="registerBtn">Зарегистрироваться</button>
        <a href="#" class="terms" @click.prevent="openTerms">Нажимая кнопку, я принимаю условия политики и пользовательского соглашения</a>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    </div>
</template>
<script>
import { regUser } from '@/services/apiUser';

export default {
    data() {
    return {
      login: '',
      password: '',
      repeatPassword: '',
      errorMessage: '',
      pdfUrl: '/src/assets/Document.pdf' 
    };
  },
  methods: {
    openTerms() {
      window.open(this.pdfUrl, '_blank'); 
    },
    async registerUser (){
        const responseUser = {
            Login: this.login,
            Password: this.password,
            RepeatPassword: this.repeatPassword
        };
        if (this.login.length <= 4 || this.login.length >= 10 || this.password.length < 8) {
                this.errorMessage = "Логин или пароль введены не корректно.";
                return;
        }
        if (!this.isPasswordValid(this.password)) {
                this.errorMessage = "Пароль должен содержать как заглавные, так и строчные буквы, а также цифры.";
                return;
        }
        if (this.password !== this.repeatPassword) {
                this.errorMessage = "Пароли не совпадают";
                return;
        }
        try{
            await regUser(responseUser)
            this.goToAvtPage()

        }
        catch(error) {
            this.errorMessage = error.response?.data?.message || "Произошла ошибка при регистрации.";
        }
    },
        goToAvtPage(){
            this.$router.push('/avt')
        },
        isPasswordValid(password) {
            const hasUpperCase = /[A-Z]/.test(password);
            const hasLowerCase = /[a-z]/.test(password);
            const hasDigit = /[0-9]/.test(password);
            return hasUpperCase && hasLowerCase && hasDigit;
        }
    }
}
</script>   
<style>
.error-message {
    color: red;
    margin-top: 10px; 
}
.panelReg {
    background-color: #f9f9f9;
    border-radius: 45px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    display: flex;
    flex-direction: column; 
    padding: 2%;
    max-width: 300px;
    height: auto; 
    margin: 8% auto 1%;
    justify-content: flex-start; 
    align-items: center; 
}
.createAcc{
font-size:1.8em;
margin-bottom: 5%;
}
.inputField {
    width: 95%; 
    padding: 3.5%;
    margin: 3% 0;
    border: 1px solid #999999;
    border-radius: 15px; 
    background-color: #f1f1f1;
}
.registerBtn {
    background-color: #ffffff; 
    color: #331901; 
    padding:3.5% 25%;
    border: none;
    border-radius: 15px; 
    cursor: pointer;
    margin: 5% 0;
    border: 1px solid #999999;
    font-size: 1em;
}

.registerBtn:hover {
    background-color: #331901;
    color: white;
}

.terms {
    font-size: 0.7em; 
    margin-top: 1%;
    color: #331901;
}
@media (max-width: 768px) {
    h1 {
        font-size: 1.5em;
    }
    .panelReg{
        padding: 4%;
    }
}
</style> 