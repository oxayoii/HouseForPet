<template>
  <div v-if="isOpen" class="modal-overlay" @click.self="closeModal">
    <div class="modal-content">
      <h2>Отсканируйте QR-код и помогите приюту!</h2>
      <img class="qrImage" src="/src/assets/qr.jpg"/>
      <div class="button-container2">
        <button class="close-button" @click="closeModal">Закрыть</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {isOpen: false};
  },
  methods: {
    openModal() {
      this.isOpen = true;
    },
    closeModal() {
      this.isOpen = false;
    }
  }
};
</script>

<style>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

/* Центрированная модалка */
.modal-content {
  background-color: white;
  padding: 20px;
  border-radius: 8px;
  text-align: center;
  max-width: 400px;
  width: 90%;
  box-sizing: border-box;
  animation: fadeIn 0.2s ease-out;
}

.qrImage {
  width: 100%;
  max-width: 300px;
  height: auto;
  margin: 20px 0;
}

.button-container2 {
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.close-button {
  background-color: white;
  color: #331901;
  border: 1px solid #331901;
  padding: 10px 16px;
  border-radius: 5px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s, color 0.3s;
}

.close-button:hover {
  background-color: #331901;
  color: white;
}

/* Fade-in animation */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.96);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

/* Адаптив */
@media (max-width: 600px) {
  .modal-content {
    padding: 15px;
  }

  .qrImage {
    max-width: 80%;
  }

  .close-button {
    padding: 8px 12px;
  }
}
</style>
