export type AdminLang = 'ru' | 'uk' | 'en'

export const ADMIN_LANG_KEY = 'dvizh-admin-lang'
export const DEFAULT_ADMIN_LANG: AdminLang = 'ru'

export const LANG_LABELS: Record<AdminLang, string> = {
  ru: 'Русский',
  uk: 'Українська',
  en: 'English',
}

export const translations = {
  ru: {
    invite: {
      yes: 'Да! 🎉',
      no: 'Нет...',
      changeAnswer: 'Изменить ответ',
      answeredYes: 'Ты сказал да!',
      answeredNo: 'Ты сказал нет...',
      expired: 'Приглашение истекло',
      notFound: 'Приглашение не найдено',
    },
    admin: {
      title: 'Приглашения',
      newInvite: 'Создать',
      noInvites: 'Нет приглашений',
      columns: {
        message: 'Сообщение',
        code: 'Ссылка',
        answer: 'Ответ',
        expiresAt: 'Истекает',
        createdAt: 'Создано',
        actions: '',
      },
      answers: {
        0: 'Ожидает',
        1: 'Да ✓',
        2: 'Нет ✗',
      } as Record<number, string>,
      confirmDelete: {
        title: 'Удалить приглашение?',
        description: 'Это действие нельзя отменить.',
        confirm: 'Удалить',
        cancel: 'Отмена',
      },
      confirmReset: {
        title: 'Сбросить ответ?',
        description: 'Вы уверены, что хотите сбросить ответ на это приглашение?',
        confirm: 'Сбросить',
        cancel: 'Отмена',
      },
      form: {
        createTitle: 'Новое приглашение',
        editTitle: 'Редактировать приглашение',
        message: 'Сообщение',
        messagePlaceholder: 'Привет! Ты придёшь?',
        description: 'Описание',
        descriptionPlaceholder: 'Необязательные детали…',
        expiresAt: 'Истекает',
        language: 'Язык',
        save: 'Сохранить',
        cancel: 'Отмена',
        languages: {
          0: 'Русский',
          1: 'Українська',
          2: 'English',
        } as Record<number, string>,
      },
      pagination: {
        previous: 'Назад',
        next: 'Вперёд',
        pageOf: (page: number, total: number) => `${page} / ${total}`,
      },
      copied: 'Ссылка скопирована',
      actions: {
        copy: 'Скопировать ссылку',
        open: 'Открыть',
        edit: 'Редактировать',
        reset: 'Сбросить ответ',
        delete: 'Удалить',
      },
    },
  },

  uk: {
    invite: {
      yes: 'Так! 🎉',
      no: 'Ні...',
      changeAnswer: 'Змінити відповідь',
      answeredYes: 'Ти сказав так!',
      answeredNo: 'Ти сказав ні...',
      expired: 'Запрошення минуло',
      notFound: 'Запрошення не знайдено',
    },
    admin: {
      title: 'Запрошення',
      newInvite: 'Створити',
      noInvites: 'Немає запрошень',
      columns: {
        message: 'Повідомлення',
        code: 'Посилання',
        answer: 'Відповідь',
        expiresAt: 'Закінчується',
        createdAt: 'Створено',
        actions: '',
      },
      answers: {
        0: 'Очікує',
        1: 'Так ✓',
        2: 'Ні ✗',
      } as Record<number, string>,
      confirmDelete: {
        title: 'Видалити запрошення?',
        description: 'Цю дію не можна скасувати.',
        confirm: 'Видалити',
        cancel: 'Скасувати',
      },
      confirmReset: {
        title: 'Скинути відповідь?',
        description: 'Ви впевнені, що хочете скинути відповідь на це запрошення?',
        confirm: 'Скинути',
        cancel: 'Скасувати',
      },
      form: {
        createTitle: 'Нове запрошення',
        editTitle: 'Редагувати запрошення',
        message: 'Повідомлення',
        messagePlaceholder: 'Привіт! Ти прийдеш?',
        description: 'Опис',
        descriptionPlaceholder: 'Необовʼязкові деталі…',
        expiresAt: 'Закінчується',
        language: 'Мова',
        save: 'Зберегти',
        cancel: 'Скасувати',
        languages: {
          0: 'Русский',
          1: 'Українська',
          2: 'English',
        } as Record<number, string>,
      },
      pagination: {
        previous: 'Назад',
        next: 'Вперед',
        pageOf: (page: number, total: number) => `${page} / ${total}`,
      },
      copied: 'Посилання скопійовано',
      actions: {
        copy: 'Скопіювати посилання',
        open: 'Відкрити',
        edit: 'Редагувати',
        reset: 'Скинути відповідь',
        delete: 'Видалити',
      },
    },
  },

  en: {
    invite: {
      yes: 'Yes! 🎉',
      no: 'No...',
      changeAnswer: 'Change answer',
      answeredYes: 'You said yes!',
      answeredNo: 'You said no...',
      expired: 'Invite has expired',
      notFound: 'Invite not found',
    },
    admin: {
      title: 'Invitations',
      newInvite: 'Create',
      noInvites: 'No invitations',
      columns: {
        message: 'Message',
        code: 'Link',
        answer: 'Answer',
        expiresAt: 'Expires',
        createdAt: 'Created',
        actions: '',
      },
      answers: {
        0: 'Pending',
        1: 'Yes ✓',
        2: 'No ✗',
      } as Record<number, string>,
      confirmDelete: {
        title: 'Delete invitation?',
        description: 'This action cannot be undone.',
        confirm: 'Delete',
        cancel: 'Cancel',
      },
      confirmReset: {
        title: 'Reset answer?',
        description: 'Are you sure you want to reset the answer for this invitation?',
        confirm: 'Reset',
        cancel: 'Cancel',
      },
      form: {
        createTitle: 'New invitation',
        editTitle: 'Edit invitation',
        message: 'Message',
        messagePlaceholder: 'Hey! Are you coming?',
        description: 'Description',
        descriptionPlaceholder: 'Optional details…',
        expiresAt: 'Expires at',
        language: 'Language',
        save: 'Save',
        cancel: 'Cancel',
        languages: {
          0: 'Русский',
          1: 'Українська',
          2: 'English',
        } as Record<number, string>,
      },
      pagination: {
        previous: 'Previous',
        next: 'Next',
        pageOf: (page: number, total: number) => `${page} / ${total}`,
      },
      copied: 'Link copied',
      actions: {
        copy: 'Copy link',
        open: 'Open',
        edit: 'Edit',
        reset: 'Reset answer',
        delete: 'Delete',
      },
    },
  },
} satisfies Record<AdminLang, typeof translations.ru>

export type Strings = typeof translations.ru
