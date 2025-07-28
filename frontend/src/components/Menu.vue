<!-- Header.vue -->
<template>
  <div class="header">
    <!-- Логотип -->
    <div class="logo-container">
      <img src="/pethouse_logo.ico" class="logo"/>
      <div class="phone">
        Дом
        <div>с&nbsp;хвостом</div>
      </div>
    </div>

    <!-- Кнопки на десктопе -->
    <div class="menu-container">
      <div class="button-container">
        <button @click="gotToFav" class="custom-button">ИЗБРАННЫЕ</button>
        <button @click="goToMain" class="custom-button">ГЛАВНАЯ</button>
        <button @click="openqrBlock" class="custom-button">ПОМОЩЬ</button>
        
        <button @click="authAction" id="butEntry" class="custom-button">
          {{ isLoggedIn ? 'ВЫЙТИ' : 'ВОЙТИ' }}
        </button>
      </div>
    </div>

    <!-- Бургер (только мобилка) -->
    <button class="menu-toggle" @click="toggleSidebar">☰</button>

    <!-- Сайдбар (только мобилка) -->
    <div class="sidebar" id="sidebar">
      <button class="close-button" @click="toggleSidebar">✖</button>
      <button @click="gotToFav" class="custom-button">ИЗБРАННЫЕ</button>
      <button @click="goToMain" class="custom-button">ГЛАВНАЯ</button>
      <button @click="openqrBlock" class="custom-button">ПОМОЩЬ</button>
      
      <button @click="authAction" id="butEntry" class="custom-button">
        {{ isLoggedIn ? 'ВЫЙТИ' : 'ВОЙТИ' }}
      </button>
    </div>
     <QrBlock ref="modal"/>
  </div>
</template>

<script>
import {getRefreshToken, logoutUser} from '@/services/apiUser';
import QrBlock from './qrBlock.vue';

export default {
  components: {QrBlock},
  data() {
    return {isLoggedIn: false};
  },
  mounted() {
    this.checkAuthStatus();
  },
  methods: {
    goToMain() {
      this.$router.push('/');
    },
    gotToFav() {
      this.$router.push('/fav');
    },
    openqrBlock() {
      console.log('openqrblock');
      this.$refs.modal.openModal();
    },
    async authAction() {
      if (this.isLoggedIn) {
        try {
          await logoutUser();
          this.isLoggedIn = false;
          this.$router.push('/avt');
        } catch (e) {
          console.error('Ошибка при выходе:', e);
        }
      } else {
        this.$router.push('/reg');
      }
    },
    checkAuthStatus() {
      this.isLoggedIn = !!getRefreshToken();
    },
    toggleSidebar() {
      document.getElementById('sidebar')?.classList.toggle('open');
    }
  }
};
</script>

<style>
/* --- базовые цвета/шрифты --- */
body {
  margin: 0;
  font-family: Arial, sans-serif;
  background: #fff2e5;
  color: #331901;
}

/* --- шапка --- */
.header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 65px;
  padding: 0 1em;
  box-sizing: border-box;
  background: #fff;
  box-shadow: 0 4px 7px rgba(0, 0, 0, .1);
  z-index: 1000;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

/* --- логотип --- */
.logo-container {
  display: flex;
  align-items: center
}

.logo {
  width: 60px;
  height: 60px;
  border-radius: 20%;
  margin-right: .5em
}

.phone {
  font-size: 1.2em;
  font-weight: bold
}

/* --- меню на десктопе --- */
.menu-container {
  display: flex;
  justify-content: center
}

.button-container {
  display: flex;
  gap: 1em;
  flex-wrap: wrap
}

/* --- бургер (мобила) --- */
.menu-toggle {
  display: none;
  font-size: 2em;
  background: none;
  border: none;
  cursor: pointer;
  transition: background .3s;
}

.menu-toggle:hover {
  background: #c5bfbf;
  color: #fff;
  border-radius: 50%
}

/* --- кнопки --- */
.custom-button {
  background: #fff;
  font-size: 2.2vh;
  padding: .6em 1em;
  border: none;
  border-radius: 20px;
  font-weight: bold;
  cursor: pointer;
  transition: background .3s, color .3s;
}

.custom-button:hover {
  background: #331901;
  color: #fff
}

#butEntry {
  background: #e9ded4
}

#butEntry:hover {
  background: #331901;
  color: #fff
}

/* --- сайдбар (мобила) --- */
.sidebar {
  position: fixed;
  top: 0;
  right: -250px;
  width: 250px;
  height: 100%;
  background: #fff;
  box-shadow: -2px 0 5px rgba(0, 0, 0, .5);
  transition: right .3s;
  z-index: 999;
  display: flex;
  flex-direction: column;
}

.sidebar.open {
  right: 0
}

.sidebar .custom-button {
  width: 100%;
  margin: .5em 0
}

.close-button {
  align-self: flex-end;
  background: #fff;
  color: #331901;
  border: none;
  border-radius: 50%;
  font-size: 2em;
  cursor: pointer;
  padding: .1em .3em;
  transition: background .3s;
}

.close-button:hover {
  background: #c5bfbf;
  color: #fff
}

/* --- адаптив --- */
@media (max-width: 968px) {
  .button-container {
    display: none
  }

  /* скрываем кнопки */
  .menu-toggle {
    display: block
  }

  /* показываем бургер */
}

@media (min-width: 969px) {
  .sidebar {
    display: none !important
  }

  /* САЙДБАР ПОЛНОСТЬЮ СКРЫТ НА ПК */
}

@media (max-width: 480px) {
  .header {
    height: 55px;
    padding: 0 .5em
  }

  .logo {
    width: 50px;
    height: 50px
  }

  .phone {
    font-size: .9em
  }

  .custom-button {
    font-size: 2vh
  }
}
</style>
```
