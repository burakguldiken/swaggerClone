<template>
  <v-app>
    <v-main>
      <Documentation/>
    </v-main>
  </v-app>
</template>

<script>
import Documentation from './components/documentation';

export default {
  name: 'App',

  components: {
    Documentation,
  },
};
</script>
