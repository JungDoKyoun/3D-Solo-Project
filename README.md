# 3D Action RPG Project

[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/JUl0VYEPpp8)

## 🎮 프로젝트 소개
Unity로 개발한 3D 액션 RPG 프로젝트입니다. State Pattern 기반의 캐릭터 시스템과 인벤토리 시스템을 구현했습니다.

## 👨‍💻 개발자
**정도균 (JungDoKyoun)**

## 📌 주요 기능

### 🎯 플레이어 시스템

#### 1. **State Pattern 기반 캐릭터 상태 관리**
<img width="932" height="315" alt="숲살 상태 drawio" src="https://github.com/user-attachments/assets/64945bd9-b0bc-4572-bfe7-e0ba41ed1370" />

```csharp
// 7가지 상태를 독립적으로 관리

PlayerIdleState, PlayerMoveState, PlayerSprintState, 
PlayerJumpState, PlayerAttackState, PlayerFallenState, PlayerLandingState
```
- 각 상태별로 독립된 Enter(), Exit(), Update(), Move() 메서드 구현
- 상태 전환 조건을 명확하게 분리하여 유지보수성 향상
- 새로운 상태 추가 시 기존 코드 수정 없이 확장 가능

#### 2. **물리 기반 이동 시스템**
- **경사로 처리**: 최대 35도까지의 경사면 이동 가능, 경사각에 따른 속도 조절
- **계단 오르내리기**: Ray Cast를 활용한 자연스러운 계단 이동
- **점프 시스템**: 중력과 점프력을 계산한 물리적 점프 구현
- **NavMesh 호환**: AI와의 상호작용을 위한 NavMesh 시스템 통합

### 💼 인벤토리 & 장비 시스템

#### 3. **드래그 앤 드롭 인벤토리**
```csharp
// 48칸 슬롯 기반 인벤토리
public void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
public void OnPointerDrag(InputAction.CallbackContext callback)
```
- 마우스 드래그로 아이템 위치 교환
- 인벤토리 밖으로 드래그 시 아이템 버리기 확인 팝업
- 아이템 스택 시스템 (CountableItem)
- 실시간 툴팁으로 아이템 정보 표시

#### 4. **장비 장착 시스템**
- **무기 타입별 공격 패턴**: 주먹, 칼, 창, 도끼, 활 등 5가지 무기 타입
- **실시간 스탯 적용**: 장비 장착 시 즉시 공격력/방어력 반영
- **장비 프리팹 교체**: 무기 장착 시 실제 3D 모델 변경
- **화살 시스템**: 활 사용 시 화살 소모 및 발사 메커니즘

### 👾 몬스터 AI 시스템

#### 5. **State Pattern 몬스터 AI**
```csharp
// 5가지 AI 상태
MonsterIdle → MonsterPatrol → MonsterChase → MonsterAttack → MonsterDie
```
- **플레이어 감지**: OverlapSphere를 통한 감지 범위 내 플레이어 탐색
- **추적 시스템**: NavMeshAgent를 활용한 장애물 회피 추적
- **전투 패턴**: 공격 범위 진입 시 자동 공격, 쿨타임 관리
- **순찰 시스템**: 랜덤 포인트 생성으로 자연스러운 순찰 동작

#### 6. **Object Pooling 몬스터 스폰**
```csharp
Dictionary<int, ObjectPool<GameObject>> monsterPool
```
- **메모리 최적화**: 미리 생성한 몬스터 재활용으로 GC 부담 감소
- **아이템 드롭**: 확률 기반 다중 아이템 드롭 시스템
- **리스폰 시스템**: 죽은 몬스터 10초 후 자동 리스폰
- **HP바 UI**: 몬스터별 독립적인 체력바 표시

## 🎮 조작법

| 키/버튼 | 동작 |
|---------|------|
| W, A, S, D | 캐릭터 이동 |
| 마우스 이동 | 시점 회전 |
| Space | 점프 |
| Left Shift | 달리기 |
| 마우스 좌클릭 | 공격 |
| I | 인벤토리 열기 |
| C | 장비창 열기 |
| ESC | UI 닫기 |

## 🛠 기술 스택
- **엔진**: Unity 2021.3 LTS
- **언어**: C#
- **디자인 패턴**: State Pattern, Singleton Pattern, Object Pooling
- **도구**: Visual Studio 2022, Git, GitHub

## 📚 주요 학습 내용
- Unity Animator Controller를 활용한 블렌드 트리 애니메이션
- New Input System을 통한 확장 가능한 입력 처리
- 인터페이스와 추상 클래스를 활용한 OOP 설계
- ScriptableObject를 통한 데이터 관리
- Rigidbody와 Collider를 활용한 물리 상호작용

## 🔧 개선 예정 사항
- Photon을 활용한 멀티플레이어 기능
- 스킬 시스템 및 콤보 공격
- 퀘스트 시스템 구현
- 던전 생성 알고리즘 추가
- 상점 및 제작 시스템

## 📄 라이선스
This project is licensed under the MIT License
