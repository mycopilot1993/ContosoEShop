(function(){
  const storageKey = 'site-theme';
  const root = document.documentElement;

  function applyTheme(name){
    // remove existing theme classes
    root.classList.remove('theme-slate','theme-sunrise','theme-forest','theme-ocean','theme-pastel');
    if(name && name !== 'default'){
      root.classList.add(name);
    }
    localStorage.setItem(storageKey, name || 'default');
  }

  function init(){
    const saved = localStorage.getItem(storageKey) || 'default';
    applyTheme(saved);
    const picker = document.getElementById('theme-picker');
    if(picker){
      const buttons = picker.querySelectorAll('.theme-swatch');
      buttons.forEach(b => {
        if(b.dataset && b.dataset.theme === saved){
          b.classList.add('active');
          b.setAttribute('aria-pressed','true');
        } else {
          b.setAttribute('aria-pressed','false');
        }
        const activate = () => {
          buttons.forEach(x => { x.classList.remove('active'); x.setAttribute('aria-pressed','false'); });
          b.classList.add('active');
          b.setAttribute('aria-pressed','true');
          applyTheme(b.dataset.theme);
        };
        b.addEventListener('click', activate);
        b.addEventListener('keydown', (e) => {
          if(e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            activate();
          }
        });
      });
    }
  }

  // expose applyTheme for console or other scripts
  window.applyTheme = applyTheme;
  document.addEventListener('DOMContentLoaded', init);
})();
