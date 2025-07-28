<template>
  <div class="panelAvt">
    <h1>ВОЙДИТЕ В АККАУНТ</h1>

    <input v-model="login" type="text" placeholder="Логин" class="inputFieldAvt" />
    <input v-model="password" type="password" placeholder="Пароль" class="inputFieldAvt" />

    <!-- Капча -->
    <div class="captcha-container">
      <p>{{ captchaQuestion }}</p>
      <input v-model="captchaInput" type="number" placeholder="Решите пример" class="inputFieldAvt" />
    </div>
    <!-- /Капча -->

    <button @click="entryUser" :disabled="loading" class="avtBtn">
      {{ loading ? 'Загрузка...' : 'Войти' }}
    </button>
    <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
  </div>
</template>

<script>
import { loginUser, getUserPermissions } from '@/services/apiUser';
import { getCaptcha } from '@/services/apiCaptcha'; 
export default {
  data() {
    return {
      login: '',
      password: '',
      loading: false,
      errorMessage: '',

      captchaQuestion: '',
      captchaInput: '',
      captchaToken: '',
    };
  },
  async mounted() {
    await this.loadCaptcha(); // Загружаем капчу при монтировании компонента
  },
  methods: {
    async loadCaptcha() {
      try {
        const captchaData = await getCaptcha(); // Получаем данные капчи из сервиса
        this.captchaQuestion = captchaData.question;
        this.captchaToken = captchaData.token;
      } catch (error) {
        console.error('Ошибка при загрузке капчи:', error);
        this.errorMessage = 'Не удалось загрузить капчу. Пожалуйста, обновите страницу.';
      }
    },
    async entryUser() {
      this.loading = true;
      this.errorMessage = '';

      try {
        // Отправляем данные для аутентификации, включая капчу
        await loginUser({
          Login: this.login,
          Password: this.password,
          CaptchaInput: String(this.captchaInput),  // Введенный пользователем ответ
          CaptchaToken: this.captchaToken,  // Токен капчи
        });

        const permissions = await getUserPermissions();
        const isAdmin = [1, 2, 3].some(p => permissions.has(p));
        this.$router.push(isAdmin ? '/admin' : '/');
      } catch (e) {
        this.errorMessage = e.response?.data?.message || 'Ошибка авторизации.';
        await this.loadCaptcha();  // Обновляем капчу при ошибке авторизации
      } finally {
        this.loading = false;
        this.captchaInput = ''; // Очищаем поле ввода капчи
      }
    },
  },
};
</script>

<style scoped>
.panelAvt {
  background-color: #f9f9f9;
  border-radius: 45px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  padding: 4%;
  max-width: 320px;
  margin: 8% auto 1%;
  justify-content: flex-start;
  align-items: center;
}

.panelAvt h1 {
  font-size: 1.7em;
  margin-bottom: 5%;
}

.inputFieldAvt {
  width: 95%;
  padding: 3.5%;
  margin: 3% 0;
  border: 1px solid #999999;
  border-radius: 15px;
  background-color: #f1f1f1;
}

.avtBtn {
  background-color: #ffffff;
  color: #331901;
  padding: 3.5% 25%;
  border: none;
  border-radius: 15px;
  cursor: pointer;
  margin: 5% 0 10px;
  border: 1px solid #999999;
  font-size: 1em;
}

.avtBtn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.captcha-container {
  margin: 10px 0;
  display: flex;
  flex-direction: column; /* Изменено на column для центрирования */
  align-items: center; /* Центрирование по горизонтали */
  width: 100%;
}

.captcha-block {
  margin: 10px 0;
  display: flex;
  justify-content: center;
  width: 100%;
}

.error-message {
  color: red;
  margin-top: 10px;
  text-align: center;
}
</style>
