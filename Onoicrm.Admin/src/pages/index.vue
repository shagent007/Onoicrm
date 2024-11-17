<template>
  <div v-for="section in config.navigation" :key="section.label" class="surface-ground">
    <h1>{{ section.label }}</h1>
    <div class="grid">
      <RouterLink :to="item.to" v-for="item in section.items" class="col-12 lg:col-4 p-3">
        <div class="p-3 border-round shadow-2 flex align-items-center surface-card">
          <div style="width: 48px; height: 48px; border-radius: 10px" class="bg-teal-100 inline-flex align-items-center justify-content-center mr-3">
            <i class="pi text-teal-600 text-3xl" :class="`${item.icon}`"></i>
          </div>
          <div>
            <span class="text-900 text-xl font-medium mb-2">{{ item.label }}</span>
            <RouterLink :to="item.to" class="block mt-1 mb-0 text-600 font-medium text-sm">Перейти</RouterLink>
          </div>
        </div>
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
const config = {
  navigation: [
    {
      label: 'Система управления',
      items: [
        {
          label: 'Клиники',
          icon: 'pi pi-fw pi-building',
          to: '/clinics',
        },
        {
          label: 'Симптомы',
          icon: 'pi pi-fw pi-check-square',
          to: '/symptoms',
        },
        {
          label: 'Зубы',
          icon: 'fas fa-tooth pl-1 pr-1',
          to: '/teeth',
        },
        {
          label: 'Состоянии зуба',
          icon: 'pi pi-sync pl-1 pr-1',
          to: '/tooth-states',
        },
        {
          "label": "Причины отмены",
          "icon": "pi pi-times-circle pl-1 pr-1",
          "to": "/cancellation-reason"
        },
        {
          label: 'Источники информации',
          icon: 'pi pi-book',
          to: '/information-sources',
        },
      ],
    },
  ],
};
</script>
