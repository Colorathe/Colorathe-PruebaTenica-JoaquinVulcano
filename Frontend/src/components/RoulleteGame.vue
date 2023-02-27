<script>
export default {
  data() {
    return {
      playerName: "",
      userBalance: 0,
      playerBet: 0,
      playerBetType: "color",
      playerBetColor: "red",
      playerBetEvenOdd: "even",
      playerBetNumber: 1,
      gameStarted: false,
      rouletteResult: null,
      betResult: null,
    };
  },
  methods: {
    startGame(initialBalance, bet) {
      // Si el usuario decide cargar su saldo usando su nombre, buscamos su saldo actual
      // y restamos la apuesta de este saldo
      if (this.loadFromUsername) {
        const userBalance = this.loadGame(this.username);
        this.balance = userBalance - bet;
      } else {
        this.balance = initialBalance - bet;
      }

      // Generamos el resultado de la ruleta y lo mostramos en la interfaz
      const result = this.generateResult();
      this.numberResult = result.number;
      this.colorResult = result.color;
      this.parImparResult = result.parImpar;
    },

    placeBet(betType) {
      // Calculamos si el usuario ha ganado o no la apuesta
      let winnings = 0;
      if (betType === "color" && this.selectedColor === this.colorResult) {
        winnings = this.betAmount * 2;
      } else if (
        betType === "parImpar" &&
        this.selectedParImpar === this.parImparResult
      ) {
        winnings = this.betAmount * 2;
      } else if (
        betType === "numberColor" &&
        this.selectedNumber === this.numberResult &&
        this.selectedColor === this.colorResult
      ) {
        winnings = this.betAmount * 10;
      }

      // Actualizamos el saldo del usuario y mostramos el resultado de la apuesta en la interfaz
      this.balance += winnings;
      this.betResult = winnings > 0 ? `Ganaste ${winnings}` : "Perdiste";
    },

    saveGame() {
      // Guardamos los datos del juego actual en una base de datos
      // asignando el monto ganado al nombre del usuario
      const userBalance = this.loadGame(this.username);
      const newBalance = userBalance + this.balance;
      this.saveToDatabase(
        this.username,
        newBalance,
        this.betAmount,
        this.balance - this.betAmount
      );
    },

    loadGame(username) {
      // Buscamos en una base de datos el saldo actual del usuario y lo devolvemos
      return this.loadFromDatabase(username);
    },
  },
};
</script>

<template>
  <div>
    <h1>Juego de Ruleta</h1>
    <div v-if="!gameStarted">
      <label>Ingresa tu nombre:</label>
      <input type="text" v-model="playerName" />
      <label>Ingresa tu saldo inicial:</label>
      <input type="number" v-model="userBalance" />
      <label>Ingresa tu apuesta:</label>
      <input type="number" v-model="playerBet" />
      <button @click="startGame">Comenzar juego</button>
    </div>
    <div v-else>
      <h2>Resultado: {{ rouletteResult }}</h2>
      <h3>
        El número es {{ isEven(rouletteResult) ? "par" : "impar" }} y es de
        color {{ isRed(rouletteResult) ? "rojo" : "negro" }}
      </h3>
      <label>Selecciona tu apuesta:</label>
      <select v-model="playerBetType">
        <option value="color">Color</option>
        <option value="even-odd">Par/Impar</option>
        <option value="number">Número</option>
      </select>
      <div v-if="playerBetType === 'color'">
        <label>Selecciona el color:</label>
        <select v-model="playerBetColor">
          <option value="red">Rojo</option>
          <option value="black">Negro</option>
        </select>
      </div>
      <div v-else-if="playerBetType === 'even-odd'">
        <label>Selecciona par o impar:</label>
        <select v-model="playerBetEvenOdd">
          <option value="even">Par</option>
          <option value="odd">Impar</option>
        </select>
        <label>Selecciona el color:</label>
        <select v-model="playerBetColor">
          <option value="red">Rojo</option>
          <option value="black">Negro</option>
        </select>
      </div>
      <div v-else-if="playerBetType === 'number'">
        <label>Selecciona el número:</label>
        <input type="number" v-model="playerBetNumber" />
        <label>Selecciona el color:</label>
        <select v-model="playerBetColor">
          <option value="red">Rojo</option>
          <option value="black">Negro</option>
        </select>
      </div>
      <button @click="placeBet">Realizar apuesta</button>
      <h3 v-if="betResult !== null">
        Resultado: {{ betResult ? "Ganaste" : "Perdiste" }}
      </h3>
      <button @click="saveBalance">Guardar saldo</button>
    </div>
  </div>
</template>

<style scoped></style>
