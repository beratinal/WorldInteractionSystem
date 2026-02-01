namespace InteractionSystem.Runtime.Core
{
    /// Oyuncunun etkileþime geçebileceði tüm nesnelerin (Kapý, Sandýk, Anahtar vb.)
    /// uygulamasý gereken temel sözleþme.
    public interface IInteractable
    {
       
        /// UI üzerinde oyuncuya gösterilecek etkileþim metni.
        /// Örn: "Press E to Open", "Pick up Key"
     
        string InteractionPrompt { get; }

        
        /// Oyuncu etkileþim tuþuna (E) bastýðýnda tetiklenir.
      
        void OnInteract();

       
        /// Raycast bu nesneye bakmaya baþladýðýnda tetiklenir.
    
        
        void OnFocus();

        /// Oyuncu bu nesneye bakmayý býraktýðýnda tetiklenir.
     
        void OnLoseFocus();
    }
}