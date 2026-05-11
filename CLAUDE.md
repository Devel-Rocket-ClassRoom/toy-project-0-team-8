# CLAUDE.md

Claude Code가 이 저장소에서 작업할 때 참고하는 가이드입니다.

## 프로젝트 개요

**ToyProject_team8** — Unity 6000.3.11f1 기반 쿠키런 스타일 2D 사이드스크롤 모바일(Android) 게임.
쿠키 캐릭터로 장애물을 피하며 달리고, 가차로 쿠키/장비를 수집하며, 스테이지를 자동 진행하는 엔들리스 러너.

**주요 특징:**
- 플레이어블 쿠키 3종: 해적(부활), 영웅(비행), 체리(폭탄 투척)
- 소모 아이템: 자석, 대시, 거대화, 포션, 코인 변환 등
- 장착 장비(아티팩트): 최대 3슬롯 동시 장착
- 다중 스테이지 + 스크롤 배경 자동 진행
- JSON 세이브/로드 (버전 마이그레이션 지원)
- 가챠 시스템 (쿠키/장비 가중치 RNG)

## 아키텍처

### 씬 흐름
`Lobby → GachaScene → ReadyScene → PlayScene → GameResultScene`

### 캐릭터 시스템
- `CookieBehavior` (추상 클래스) ← `HeroCookie`, `CherryCookie`, `PirateCookieBehavior`
- `CookieBehaviorFactory`: `CookieType` 기반 정적 팩토리 패턴
- `CookieController`: 물리, 입력, 상태머신(Run/Jump/DoubleJump/Slide/Death/Revive), 체력 관리

### 주요 매니저
| 클래스 | 역할 |
|---|---|
| `GameManager` | 세션 전체 제어 (스테이지 로딩, 스크롤 속도, 파워업, 점수) |
| `SaveLoadManager` | JSON 직렬화/역직렬화 (정적 클래스) |
| `DataTableManager` | CSV 테이블 캐시 (CookieTable, GearTable) |
| `PlayDataManager` | 씬 간 세션 데이터 싱글톤 (DontDestroyOnLoad) |

### 아이템/장비
- `ItemBase` (추상): 충돌 시 효과 적용 후 소멸하는 소모 아이템
- `GearBase` (추상): 게임 중 지속되는 장착형 아티팩트, `InGameGearSlot`이 프로그레스바 시각화

### 상태 플래그 (`GameManager`)
- `ScrollObjectsFlag`: 맵/배경 이동 전체 제어
- `GameEndFlag`: 게임 종료 후 입력/물리 차단

## 파일 구조

```
Assets/Scripts/
├── Character/          # 쿠키 동작 및 제어
│   ├── CookieController.cs
│   ├── CookieBehavior.cs / CookieBehaviorFactory.cs
│   ├── Hero/, Cherry/, Pirate/
├── GamePlay/           # 세션 관리
│   ├── GameManager.cs / PlayDataManager.cs
│   ├── InGameGearSlot.cs / Background.cs
├── Item/
│   ├── Object/         # ItemBase 및 소모 아이템 구현체
│   └── Gear/           # GearBase 및 장비 구현체
├── Data/
│   ├── SaveLoadManager.cs / DataTableManager.cs
│   ├── Cookie/, Object/  # 테이블 및 세이브 구조체
├── Map/                # StageData SO, MapPrefab, ScrollObject
├── Ui/                 # Lobby, Play, Gacha UI
└── Constants/          # Tags.cs, CookieType, CookieState 열거형
```

## 주요 통합 포인트

### 초기화 흐름
1. `GameManager.LoadCharacter(CookieData)` 호출
2. `CookieController.Init()` → `CookieBehaviorFactory`가 행동 컴포넌트 추가
3. `Resources/Animations/Character/{Type}/`에서 Animator 로드
4. 장비 인스턴스 생성 후 `InGameGearSlot`에 등록

### 데미지 처리
- 매 프레임 `_healthReduceSpeed`만큼 HP 감소
- 충돌 시: 추가 HP → 기본 HP 순서로 감소, 2초 무적 발동
- 해적 쿠키는 첫 사망 시 부활 (고스트 애니메이터로 교체)

### 파워업 흐름
`GameManager.ActivateDash/Giant/Magnet(duration)` → 코루틴으로 효과 적용 → 종료 후 1초 무적

## 개발 작업 가이드

### 새 쿠키 추가
1. `CookieBehavior` 서브클래스 생성
2. `CookieType` 열거형 값 추가
3. `CookieBehaviorFactory` switch문 업데이트
4. `Resources/Animations/Character/{NewType}/`에 Animator 추가
5. `CookieTable.csv`에 행 추가

### 새 소모 아이템 추가
1. `ItemBase` 상속
2. `ItemDuration` 프로퍼티 정의
3. `ApplyItemEffect()` 오버라이드 (`GameManager` 메서드 호출)

### 새 장비 추가
1. `GearBase` 상속
2. `GetProgressBarAmount()` 구현 (0–1 쿨다운 반환)
3. `GearTable.csv`에 행 추가

### 밸런스 수정
- 스크롤 속도: `StageData.ScrollSpeed`
- 쿠키 체력: `CookieTable.csv` Hp 컬럼
- 아이템 지속시간: 각 `ItemBase` 서브클래스의 `ItemDuration`
- 파워업 배율: `GameManager` 상수 (`_giantScaleMultiplier`, `_dashSpeedMultiplier`)
- 가챠 확률: `GachaCookie` / `GachaGear` ScriptableObject 가중치 값

## 알려진 이슈

- `StageChanger.WaitChangeStage()` 코루틴이 `LoadNextStage()` 미호출 (스테이지 전환 미완성)
- `SaveLoadManager`의 암호화 모드 미구현
- `CookieController.SetGear()`에 `FindGameObjectWithTag("SceneManager")` 하드코딩 의존성

## GitHub 워크플로우

- 브랜치: `feature/*` → PR → main
- PR 템플릿: 작업 브랜치명, 요약, 변경사항, 관련 이슈(`close #123`), 체크리스트(빌드 확인, 디버그 코드 제거, 메타 파일 검토)
