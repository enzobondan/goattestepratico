/** @type {import('tailwindcss').Config} */
const { addDynamicIconSelectors } = require("@iconify/tailwind");
import daisyui from "daisyui";
export default {
  content: ["./src/**/*.{html,ts}"],
  safelist: [
    "icon-[mdi-light--file]",
    "icon-[uil--award]",
    "icon-[bi--gear]",
    "icon-[ion--water-outline]",
    "icon-[hugeicons--city-03]",
    "icon-[tabler--home]",
  ],
  theme: {
    extend: {
      fontFamily: {
        inter: ["Inter", "sans-serif"],
      },
    },
  },
  plugins: [daisyui, addDynamicIconSelectors()],
  daisyui: {
    themes: ["light"],
  },
};
